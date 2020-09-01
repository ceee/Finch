using FluentValidation;
using FluentValidation.Results;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Api
{
  public abstract class AppAwareBackofficeApi : BackofficeApi, IAppAwareBackofficeApi
  {
    protected bool AllowShared { get; set; } = false;

    public AppAwareBackofficeApi(IBackofficeStore store) : base(store)
    {
      Scope = new ApiScope()
      {
        AppId = store.AppContext.AppId,
        IncludeShared = false // TODO based on user
      };
    }
  }


  public abstract class BackofficeApi : IBackofficeApi
  {
    public ApiScope Scope { get; protected set; }

    protected IDocumentStore Raven { get; private set; }

    const string NEW_ID = "new:";

    protected IBackofficeStore Backoffice { get; private set; }


    public BackofficeApi(IBackofficeStore store)
    {
      Raven = store.Raven;
      Backoffice = store;
      Scope = new ApiScope()
      {
        IsShared = true
      };
    }


    /// <inheritdoc />
    public async Task<T> GetById<T>(string id) where T : IZeroIdEntity
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      T model = await session.LoadAsync<T>(id);
      IAppAwareEntity appAwareModel = model as IAppAwareEntity;

      if (model == null || appAwareModel == null)
      {
        return model;
      }

      if (!Scope.IsAllowed(appAwareModel.AppId))
      {
        return default;
      }

      return model;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, T>> GetByIds<T>(params string[] ids) where T : IZeroIdEntity
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      Dictionary<string, T> models = await session.LoadAsync<T>(ids);
      Dictionary<string, T> result = new Dictionary<string, T>();

      foreach (string id in ids)
      {
        if (!models.TryGetValue(id, out T model))
        {
          result.Add(id, default);
          continue;
        }

        IAppAwareEntity appAwareModel = model as IAppAwareEntity;

        if (appAwareModel == null || !Scope.IsAllowed(appAwareModel.AppId))
        {
          result.Add(id, default);
          continue;
        }

        result.Add(id, model);
      }

      return result;
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> SaveModel<T>(T model, IValidator<T> validator = null) where T : IZeroIdEntity
    {
      // check for alias
      //if (model is IUrlAliasEntity)
      //{
      //  IUrlAliasEntity entity = operation.Model as IUrlAliasEntity;
      //  entity.Alias = entity.Alias?.ToLower().ToUrlSegment();
      //}

      // get specifics 
      IAppAwareEntity appAwareEntity = model as IAppAwareEntity;
      IZeroEntity zeroEntity = model as IZeroEntity;

      // run validator
      if (validator != null)
      {
        ValidationResult validation = await validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      // set app id
      if (appAwareEntity != null && appAwareEntity.AppId.IsNullOrEmpty())
      {
        appAwareEntity.AppId = Scope.AppId; // Constants.Database.SharedAppId; // TODO correct app id
      }

      // check if current app id is valid
      if (!model.Id.IsNullOrEmpty() && Scope.IsAppAware && appAwareEntity != null)
      {
        if (!Scope.IsAllowed(appAwareEntity.AppId))
        {
          return EntityResult<T>.Fail("@errors.onsave.notallowed");
        }
      }

      // find all media items in model
      //List<ObjectTraverser.Result<Media>> media = ObjectTraverser.Find<Media>(model);

      // upload media items
      //Dictionary<string, Media> mediaItems = new Dictionary<string, Media>();

      //foreach (ObjectTraverser.Result<Media> item in media)
      //{
      //  string id = item.Item?.Id;

      //  if (!Media.Upload(item.Item, out bool uploaded, out string uploadError))
      //  {
      //    return EntityResult<T>.Fail(item.Path, uploadError);
      //  }
      //  else
      //  {
      //    mediaItems.Add(id, item.Item);
      //  }
      //}

      //if (operation.Media != null)
      //{
      //  operation.Media?.Invoke(operation.Model, mediaItems);
      //}

      // find all Raven Ids
      List<ObjectTraverser.Result<GenerateIdAttribute>> ravenIds = ObjectTraverser.FindAttribute<GenerateIdAttribute>(model);

      // set unset Raven Ids
      foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ravenIds)
      {
        string id = item.Property.GetValue(item.Parent, null) as string;
        if (String.IsNullOrWhiteSpace(id) || id.StartsWith(NEW_ID))
        {
          item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? Raven.Id(item.Item.Length.Value) : Raven.Id());
        }
      }

      // get current user
      string userId = Backoffice.Auth.GetUserId();

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        if (zeroEntity != null)
        {
          zeroEntity.CreatedDate = DateTimeOffset.Now;
          zeroEntity.CreatedById = userId;
        }

        if (model is ILanguageAwareEntity)
        {
          (model as ILanguageAwareEntity).LanguageId = "languages.1-A"; // TODO correct language id
        }
      }

      // update name alias and last modified
      if (zeroEntity != null)
      {
        zeroEntity.Alias = Safenames.Alias(zeroEntity.Name);
        zeroEntity.LastModifiedById = userId;
        zeroEntity.CreatedById ??= userId;
      }

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

      await session.StoreAsync(model);
      await session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> DeleteById<T>(string id) where T : IZeroIdEntity
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

      T entity = await session.LoadAsync<T>(id);
      IAppAwareEntity appAwareEntity = entity as IAppAwareEntity;

      if (entity == null)
      {
        return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
      }

      if (appAwareEntity != null && Scope.IsAppAware && !Scope.IsAllowed(appAwareEntity.AppId))
      {
        return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
      }

      session.Delete(entity);

      await session.SaveChangesAsync();

      return EntityResult<T>.Success();
    }


    /// <inheritdoc />
    public async Task<int> DeleteByIds<T>(params string[] ids) where T : IZeroIdEntity
    {
      int successCount = 0;

      foreach (string id in ids)
      {
        EntityResult<T> result = await DeleteById<T>(id);
        successCount += result.IsSuccess ? 1 : 0;
      }

      return successCount;
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Purge<T>(string querySuffix = null, Parameters parameters = null)
    {
      var collectionName = Raven.Conventions.FindCollectionName(typeof(T));

      var operationQuery = new DeleteByQueryOperation(new IndexQuery()
      {
        Query = $"from {collectionName} c {querySuffix ?? String.Empty}",
        QueryParameters = parameters
      });

      Operation operation = await Raven.Operations.SendAsync(operationQuery);

      await operation.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

      return EntityResult<T>.Success();
    }
  }


  public interface IBackofficeApi
  {
    /// <summary>
    /// Get an entity by Id.
    /// If the requested entity is an IAppAwareEntity it will only return entities for the currently selected app + shared app
    /// </summary>
    Task<T> GetById<T>(string id) where T : IZeroIdEntity;

    /// <summary>
    /// Get entities by ids.
    /// If the requested entity is an IAppAwareEntity it will only return entities for the currently selected app + shared app
    /// </summary>
    Task<Dictionary<string, T>> GetByIds<T>(params string[] ids) where T : IZeroIdEntity;

    /// <summary>
    /// Updates or creates an entity with an optional validator
    /// </summary>
    Task<EntityResult<T>> SaveModel<T>(T model, IValidator<T> validator = null) where T : IZeroIdEntity;

    /// <summary>
    /// Deletes an entity by Id
    /// </summary>
    Task<EntityResult<T>> DeleteById<T>(string id) where T : IZeroIdEntity;

    /// <summary>
    /// Deletes entities by Id
    /// </summary>
    Task<int> DeleteByIds<T>(params string[] ids) where T : IZeroIdEntity;

    /// <summary>
    /// Delete a whole collection (with an optional query suffix, i.e. a where statement)
    /// </summary>
    Task<EntityResult<T>> Purge<T>(string querySuffix = null, Parameters parameters = null);
  }


  public interface IAppAwareBackofficeApi : IBackofficeApi
  {
    ApiScope Scope { get; }
  }
}
