using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Validation;
using Microsoft.AspNetCore.Hosting;


namespace zero.Core.Api
{
  public class SetupApi : ISetupApi
  {
    protected IZeroConfiguration Config { get; private set; }

    //protected UserManager<BackofficeUser> UserManager { get; private set; }


    public SetupApi(IZeroConfiguration config)
    {
      Config = config;
      //UserManager = userManager;
    }


    public async Task<EntityChangeResult<SetupModel>> Install(SetupModel model)
    {
      ValidationResult validation = await new SetupModelValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityChangeResult<SetupModel>.Fail(validation);
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

        raven.Conventions.FindCollectionName = type =>
        {
          return Constants.Database.CollectionPrefix + DocumentConventions.DefaultGetCollectionName(type);
        };

        raven.Conventions.IdentityPartsSeparator = ".";

        raven.Initialize();

        // create application
        Application app = new Application()
        {
          CreatedDate = DateTimeOffset.Now,
          IsActive = true,
          Name = model.AppName,
          Alias = Alias.Generate(model.AppName)
        };

        // create user
        User user = new User()
        {
          IsSuper = true,
          CreatedDate = DateTimeOffset.Now,
          Email = model.User.Email,
          Name = model.User.Name,
          IsActive = true,
          LanguageId = Config.DefaultLanguage,
          Alias = Alias.Generate(model.User.Name),
        };

        // save application and user
        using (IAsyncDocumentSession session = raven.OpenAsyncSession())
        {
          await session.StoreAsync(app);

          // set app-id for user
          user.AppId = session.Advanced.GetDocumentId(app);

          await session.StoreAsync(user);

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

      return EntityChangeResult<SetupModel>.Success(model);
    }


    /// <summary>
    /// Updates the settings file with the new data (database connection and version)
    /// </summary>
    void UpdateSettingsFile(SetupModel model)
    {
      var filePath = Path.Combine(model.ContentRootPath, "zeroSettings.json");
      string json = File.ReadAllText(filePath);

      ZeroConfiguration config = JsonConvert.DeserializeObject<ZeroConfiguration>(json);

      config.Raven = new RavenConfig()
      {
        Database = model.Database.Name,
        Url = model.Database.Url
      };

      config.ZeroVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

      json = JsonConvert.SerializeObject(config, Formatting.Indented);

      File.WriteAllText(filePath, json);
    }
  }



  public interface ISetupApi
  {
    Task<EntityChangeResult<SetupModel>> Install(SetupModel model);
  }
}
