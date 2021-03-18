using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Core.Utils;
using zero.Core.Validation;


namespace zero.Core.Api
{
  public class SetupApi : ISetupApi
  {
    protected IZeroOptions Options { get; private set; }

    protected IPasswordHasher<BackofficeUser> PasswordHasher { get; private set; }

    protected IZeroDocumentConventionsBuilder ConventionsBuilder { get; private set; }


    public SetupApi(IZeroOptions options, IPasswordHasher<BackofficeUser> passwordHasher, IZeroDocumentConventionsBuilder conventionsBuilder)
    {
      Options = options;
      PasswordHasher = passwordHasher;
      ConventionsBuilder = conventionsBuilder;
    }


    public async Task<EntityResult<SetupModel>> Install(SetupModel model)
    {
      ValidationResult validation = await new SetupModelValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<SetupModel>.Fail(validation);
      }

      DocumentStore raven = null;

      try
      {
        // test read & write permissions on folders
        // TODO

        // create temporary instance of database
        raven = new DocumentStore()
        {
          Urls = model.Database.Url.Split(','),
          Database = model.Database.Name
        };

        ConventionsBuilder.Run(raven.Conventions);
        raven.Initialize();

        // create application
        Application app = Prepare(new Application()
        {
          Id = "app." + Safenames.Alias(model.AppName),
          CreatedDate = DateTimeOffset.Now,
          IsActive = true,
          Name = model.AppName,
          Alias = Safenames.Alias(model.AppName),
          Database = model.Database.Name
        });

        // create user
        BackofficeUser user = Prepare(new BackofficeUser()
        {
          IsSuper = true,
          CreatedDate = DateTimeOffset.Now,
          Email = model.User.Email,
          Username = model.User.Email,
          Name = model.User.Name,
          IsActive = true,
          LanguageId = Options.DefaultLanguage,
          Alias = Safenames.Alias(model.User.Name),
          IsEmailConfirmed = true,
          SecurityStamp = NewSecurityStamp()
        });

        user.PasswordHash = PasswordHasher.HashPassword(user, model.User.Password);

        // create default language
        Language language = Prepare(new Language() // TODO get default language selection from setup UI
        {
          Name = "English",
          Alias = Safenames.Alias("English"),
          CreatedDate = DateTimeOffset.Now,
          Code = "en-US",
          IsActive = true,
          IsDefault = true
        });

        using IAsyncDocumentSession session = raven.OpenAsyncSession();

        await session.StoreAsync(user);

        // user creation failed
        //if (!result.Succeeded)
        //{
        //  EntityResult<SetupModel> entityResult = EntityResult<SetupModel>.Fail();

        //  foreach (IdentityError error in result.Errors)
        //  {
        //    entityResult.AddError(error.Code, error.Description);
        //  }

        //  return entityResult;
        //}


        await session.StoreAsync(app);

        // save default user roles
        IList<BackofficeUserRole> roles = GetRoles(model);

        foreach (BackofficeUserRole role in roles)
        {
          await session.StoreAsync(role);
        }

        // add admin role to super user
        // set app-id for user and store it
        user.AppId = session.Advanced.GetDocumentId(app);
        user.CurrentAppId = user.AppId;
        user.RoleIds.Add(roles.First(role => role.Name == "Standard").Id);
        user.RoleIds.Add(roles.First(role => role.Name == "Administrator").Id);
        await session.StoreAsync(user);

        // create language
        await session.StoreAsync(language);

        // set countries
        using (Raven.Client.Documents.BulkInsert.BulkInsertOperation bulkInsert = raven.BulkInsert())
        {
          foreach (Country country in GetCountries(model, language))
          {
            await bulkInsert.StoreAsync(country);
          }
        }

        // update settings file. if this fails the changes won't be stored
        UpdateSettingsFile(model);

        await session.SaveChangesAsync();
      }
      catch (Exception)
      {
        // TODO revert
        throw;
      }
      finally
      {
        raven?.Dispose();
      }

      return EntityResult<SetupModel>.Success(model);
    }


    /// <summary>
    /// Updates the settings file with the new data (database connection and version)
    /// </summary>
    void UpdateSettingsFile(SetupModel model)
    {
      // TODO should we write this into appSettings.json now? 
      // or let the user do it in the code editor?

      //var filePath = Path.Combine(model.ContentRootPath, "zeroSettings.json");
      //string json = File.ReadAllText(filePath);

      //ZeroConfiguration config = JsonConvert.DeserializeObject<ZeroConfiguration>(json);

      //config.Raven = new RavenOptions()
      //{
      //  Database = model.Database.Name,
      //  Url = model.Database.Url
      //};

      //config.ZeroVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

      //json = JsonConvert.SerializeObject(config, Formatting.Indented);

      //File.WriteAllText(filePath, json);
    }


    /// <summary>
    /// Get countries for supported backoffice languages
    /// </summary>
    IList<Country> GetCountries(SetupModel model, Language defaultLanguage)
    {
      List<Country> countries = new List<Country>();

      //string[] isoCodes = Config.SupportedLanguages;
      string[] isoCodes = new string[1] { "en-US" };

      foreach (string languageISO in isoCodes)
      {
        // country list from: https://github.com/umpirsky/country-list/tree/master/data
        var filePath = Path.Combine(model.ContentRootPath, "Resources", "Countries", "countries." + languageISO.ToLowerInvariant() + ".json");
        string json = File.ReadAllText(filePath);


        countries.AddRange(JsonConvert.DeserializeObject<Dictionary<string, string>>(json).Select(country => Prepare(new Country()
        {
          CreatedDate = DateTimeOffset.Now,
          IsActive = true,
          //AppId = Constants.Database.SharedAppId, // TODO appx fix
          Alias = Safenames.Alias(country.Value),
          //LanguageId = defaultLanguage.Id,
          Code = country.Key.ToLowerInvariant(),
          Name = country.Value
        })).ToList());
      }

      return countries;
    }


    /// <summary>
    /// Create default roles
    /// </summary>
    IList<BackofficeUserRole> GetRoles(SetupModel model)
    {
      string type = Constants.Auth.Claims.Permission;

      BackofficeUserRole adminRole = Prepare(new BackofficeUserRole()
      {
        Name = "Administrator",
        Alias = Safenames.Alias("Administrator"),
        Sort = 0,
        Icon = "fth-award",
        CreatedDate = DateTimeOffset.Now,
        IsActive = true,
        Claims = new List<IUserClaim>()
        {
          //new UserClaim(type, Permissions.Applications, PermissionsValue.Write),
          new UserClaim(type, Permissions.Sections.Dashboard, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Spaces, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Pages, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Media, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Settings, PermissionsValue.True),
          new UserClaim(type, Permissions.Settings.Applications, PermissionsValue.None),
          new UserClaim(type, Permissions.Settings.Countries, PermissionsValue.Update),
          new UserClaim(type, Permissions.Settings.Logging, PermissionsValue.Update),
          new UserClaim(type, Permissions.Settings.Translations, PermissionsValue.Update),
          new UserClaim(type, Permissions.Settings.Updates, PermissionsValue.Update),
          new UserClaim(type, Permissions.Settings.Users, PermissionsValue.Update),
        },
      });

      BackofficeUserRole editorRole = Prepare(new BackofficeUserRole()
      {
        Name = "Editor",
        Alias = Safenames.Alias("Editor"),
        Sort = 1,
        Icon = "fth-feather",
        CreatedDate = DateTimeOffset.Now,
        IsActive = true,
        Claims = new List<IUserClaim>()
        {
          new UserClaim(type, Permissions.Sections.Dashboard, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Spaces, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Pages, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Media, PermissionsValue.True),
          new UserClaim(type, Permissions.Sections.Settings, PermissionsValue.True),
          new UserClaim(type, Permissions.Settings.Translations, PermissionsValue.True)
        }
      });

      BackofficeUserRole defaultRole = Prepare(new BackofficeUserRole()
      {
        Name = "Standard",
        Alias = Safenames.Alias("Standard"),
        Sort = 2,
        Icon = "fth-users",
        CreatedDate = DateTimeOffset.Now,
        IsActive = true,
        Claims = new List<IUserClaim>()
        {
          new UserClaim(type, Permissions.Sections.Dashboard, PermissionsValue.True)
        }
      });

      return new List<BackofficeUserRole>() { adminRole, editorRole, defaultRole };
    }


    T Prepare<T>(T model, string languageId = null) where T : IZeroIdEntity
    {
      IZeroEntity zeroEntity = model as IZeroEntity;

      // set default properties
      if (zeroEntity != null && zeroEntity.CreatedDate == default)
      {
        zeroEntity.CreatedDate = DateTimeOffset.Now;
      }
      if (zeroEntity != null && zeroEntity.CreatedById == default)
      {
        zeroEntity.CreatedById = Constants.Auth.SystemUser;
      }

      if (model is ILanguageAwareEntity && (model as ILanguageAwareEntity).LanguageId == null)
      {
        (model as ILanguageAwareEntity).LanguageId = languageId;
      }

      // update name alias and last modified
      if (zeroEntity != null)
      {
        zeroEntity.Alias = Safenames.Alias(zeroEntity.Name);
        zeroEntity.LastModifiedById = Constants.Auth.SystemUser;
        zeroEntity.LastModifiedDate = DateTimeOffset.Now;
        zeroEntity.Hash ??= IdGenerator.Create();
        zeroEntity.IsActive = true;
      }

      return model;
    }


    /// <summary>
    /// Creates a new security stamp
    /// </summary>
    string NewSecurityStamp()
    {
      byte[] bytes = new byte[20];
      RandomNumberGenerator.Fill(bytes);
      return Base32.ToBase32(bytes);
    }
  }



  public interface ISetupApi
  {
    Task<EntityResult<SetupModel>> Install(SetupModel model);
  }
}
