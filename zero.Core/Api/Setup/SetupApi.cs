using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Entities.Setup;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class SetupApi : ISetupApi
  {
    public SetupApi()
    {
      //Raven =
    }


    public async Task<EntityChangeResult<SetupModel>> Install(SetupModel model)
    {
      ValidationResult validation = await new SetupModelValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityChangeResult<SetupModel>.Fail(validation);
      }

      // test read & write permissions on folders
      // TODO

      // create temporary instance of database
      DocumentStore raven = new DocumentStore()
      {
        Urls = model.Database.Url.Split(','),
        Database = model.Database.Name
      };

      raven.Conventions.FindCollectionName = type =>
      {
        return Constants.Database.CollectionPrefix + DocumentConventions.DefaultGetCollectionName(type);
      };

      raven.Initialize();

      // create application
      Application app = new Application()
      {
        CreatedDate = DateTimeOffset.Now,
        IsActive = true,
        Name = model.AppName,
        Alias = Alias.Generate(model.AppName)
      };

      using (IAsyncDocumentSession session = raven.OpenAsyncSession())
      {
        await session.StoreAsync(app);
        await session.SaveChangesAsync();
      }

      raven.Dispose();

      return EntityChangeResult<SetupModel>.Success(model);
    }
  }



  public interface ISetupApi
  {
    Task<EntityChangeResult<SetupModel>> Install(SetupModel model);
  }
}
