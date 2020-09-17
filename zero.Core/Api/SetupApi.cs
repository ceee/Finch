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
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Core.Validation;


namespace zero.Core.Api
{
  public class SetupApi : ISetupApi
  {
    protected IZeroOptions Options { get; private set; }

    protected UserManager<User> UserManager { get; private set; }


    public SetupApi(IZeroOptions options, UserManager<User> userManager)
    {
      Options = options;
      UserManager = userManager;
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

        raven.Setup(Options).Initialize();

        // create application
        Application app = new Application()
        {
          CreatedDate = DateTimeOffset.Now,
          IsActive = true,
          Name = model.AppName,
          Alias = Safenames.Alias(model.AppName)
        };

        // create user
        User user = new User()
        {
          IsSuper = true,
          CreatedDate = DateTimeOffset.Now,
          Email = model.User.Email,
          Name = model.User.Name,
          IsActive = true,
          LanguageId = Options.DefaultLanguage,
          Alias = Safenames.Alias(model.User.Name),
          IsEmailConfirmed = true
        };

        // create default language
        Language language = new Language() // TODO get default language selection from setup UI
        {
          Name = "English",
          Alias = Safenames.Alias("English"),
          CreatedDate = DateTimeOffset.Now,
          Code = "en-US",
          IsActive = true,
          IsDefault = true
        };

        // TODO UserManager uses the DI resolved IDocumentStore instance which should not be available at this point??
        IdentityResult result = await UserManager.CreateAsync(user, model.User.Password);

        // user creation failed
        if (!result.Succeeded)
        {
          EntityResult<SetupModel> entityResult = EntityResult<SetupModel>.Fail();

          foreach (IdentityError error in result.Errors)
          {
            entityResult.AddError(error.Code, error.Description);
          }

          return entityResult;
        }

        // save entities
        using (IAsyncDocumentSession session = raven.OpenAsyncSession())
        {
          await session.StoreAsync(app);

          // set app-id for user and store it
          user.AppId = session.Advanced.GetDocumentId(app);
          await session.StoreAsync(user);

          // save default user roles
          IList<UserRole> roles = GetRoles(model);

          foreach (UserRole role in roles)
          {
            await session.StoreAsync(role);
          }

          // add admin role to super user
          user.Roles.Add(roles.First(role => role.Name == "Standard").Alias);
          user.Roles.Add(roles.First(role => role.Name == "Administrator").Alias);

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
      }
      catch (Exception ex)
      {
        // TODO revert

        throw ex;
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


        countries.AddRange(JsonConvert.DeserializeObject<Dictionary<string, string>>(json).Select(country => new Country()
        {
          CreatedDate = DateTimeOffset.Now,
          IsActive = true,
          AppId = Constants.Database.SharedAppId,
          Alias = Safenames.Alias(country.Value),
          //LanguageId = defaultLanguage.Id,
          Code = country.Key.ToLowerInvariant(),
          Name = country.Value
        }).ToList());
      }

      return countries;
    }


    /// <summary>
    /// Create default roles
    /// </summary>
    IList<UserRole> GetRoles(SetupModel model)
    {
      string type = Constants.Auth.Claims.Permission;

      UserRole adminRole = new UserRole()
      {
        Name = "Administrator",
        Alias = Safenames.Alias("Administrator"),
        Sort = 0,
        AppId = Constants.Database.SharedAppId,
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
      };

      UserRole editorRole = new UserRole()
      {
        Name = "Editor",
        Alias = Safenames.Alias("Editor"),
        Sort = 1,
        AppId = Constants.Database.SharedAppId,
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
      };

      UserRole defaultRole = new UserRole()
      {
        Name = "Standard",
        Alias = Safenames.Alias("Standard"),
        Sort = 2,
        AppId = Constants.Database.SharedAppId,
        Icon = "fth-users",
        CreatedDate = DateTimeOffset.Now,
        IsActive = true,
        Claims = new List<IUserClaim>()
        {
          new UserClaim(type, Permissions.Sections.Dashboard, PermissionsValue.True)
        }
      };

      return new List<UserRole>() { adminRole, editorRole, defaultRole };
    }
  }



  public interface ISetupApi
  {
    Task<EntityResult<SetupModel>> Install(SetupModel model);
  }
}
