using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core
{
  public class SyncPseudo
  {
    public SyncPseudo()
    {

    }


    public async Task OnCreate<T>(IAsyncDocumentSession session, T model) where T : IZeroEntity
    {
      await OnSave<T>(session, model, true);
    }


    public async Task OnUpdate<T>(IAsyncDocumentSession session, T model) where T : IZeroEntity
    {
      await OnSave<T>(session, model, false);
    }


    async Task OnSave<T>(IAsyncDocumentSession session, T model, bool isCreate = false) where T : IZeroEntity
    {
      string sharedId = Constants.Database.SharedAppId;

      if (model.AppId != sharedId)
      {
        return;
      }

      IList<T> inheritedModels = new List<T>();
      IList<IApplication> apps = await session.Query<IApplication>().Where(x => x.Id != sharedId).ToListAsync();

      if (!isCreate)
      {
        string id = model.Id;
        inheritedModels = await session.Query<T>().Where(x => x.BlueprintId == id).ToListAsync();
      }

      foreach (IApplication app in apps)
      {
        T inheritedModel = inheritedModels.FirstOrDefault(x => x.AppId == app.Id);

        // the model does not yet exist in the app, so we need to create it
        if (inheritedModel == null)
        {
          inheritedModel = model.Clone();
          inheritedModel.Id = null;
        }
        // we need to override allowed properties in the inherited model
        else
        {
          // TODO correctly override properties
          if (inheritedModel is ITranslation)
          {
            ((ITranslation)inheritedModel).Key = ((ITranslation)model).Key;
            ((ITranslation)inheritedModel).Value = ((ITranslation)model).Value;
            ((ITranslation)inheritedModel).LastModifiedById = ((ITranslation)model).LastModifiedById;
            ((ITranslation)inheritedModel).LastModifiedDate = ((ITranslation)model).LastModifiedDate;
          }
        }
        
        inheritedModel.AppId = app.Id;
        inheritedModel.BlueprintId = model.Id;

        await session.StoreAsync(inheritedModel);
      }
    }
  }
}
