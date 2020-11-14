using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Entities.Messages;
using zero.Core.Extensions;
using zero.Core.Messages;

namespace zero.Debug.Sync
{
  public abstract class BlueprintHandler<T> : IMessageHandler<EntitySavedMessage<T>> where T : IZeroEntity
  {
    public virtual async Task Handle(EntitySavedMessage<T> message)
    {
      //await CreateBlueprints(message.Session, message.Model, message.IsCreate);
    }


    protected virtual Task OnBlueprintCreateAsync(T blueprint, T model)
    {
      return Task.CompletedTask;
    }


    protected virtual void OnBlueprintCreate(T blueprint, T model)
    {
      return;
    }


    //protected virtual async Task CreateBlueprints(IAsyncDocumentSession session, T model, bool isCreate = false)
    //{
    //  string sharedId = null; // Constants.Database.SharedAppId;

    //  if (model.AppId != sharedId)
    //  {
    //    return;
    //  }

    //  IList<T> inheritedModels = new List<T>();
    //  IList<IApplication> apps = await session.Query<IApplication>().Where(x => x.Id != sharedId).ToListAsync();

    //  if (!isCreate)
    //  {
    //    string id = model.Id;
    //    inheritedModels = await session.Query<T>().Where(x => x.Blueprint != null && x.Blueprint.Id == id).ToListAsync();
    //  }

    //  foreach (IApplication app in apps)
    //  {
    //    bool exists = true;
    //    T inheritedModel = inheritedModels.FirstOrDefault(x => x.AppId == app.Id);

    //    // the model does not yet exist in the app, so we need to create it
    //    if (inheritedModel == null)
    //    {
    //      exists = false;
    //      inheritedModel = model.Clone();
    //      inheritedModel.Id = null;
    //    }

    //    inheritedModel.AppId = app.Id;
    //    inheritedModel.LastModifiedById = model.LastModifiedById;
    //    inheritedModel.LastModifiedDate = model.LastModifiedDate;
    //    inheritedModel.Blueprint = new BlueprintConfiguration()
    //    {
    //      Id = model.Id
    //    };

    //    // we need to override allowed properties in the inherited model
    //    if (exists)
    //    {
    //      inheritedModel.Name = model.Name;
    //      inheritedModel.Alias = Safenames.Alias(model.Name);
    //      inheritedModel.IsActive = model.IsActive;
    //      inheritedModel.Sort = model.Sort;
    //      await OnBlueprintCreateAsync(model, inheritedModel);
    //      OnBlueprintCreate(model, inheritedModel);
    //    }

    //    await session.StoreAsync(inheritedModel);
    //  }
    //}
  }
}
