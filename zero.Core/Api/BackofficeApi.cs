using FluentValidation;
using FluentValidation.Results;
using Raven.Client;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Attributes;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Api
{
  public abstract class BackofficeApi : IBackofficeApi
  {
    protected IZeroStore Store { get; private set; }

    const string NEW_ID = "new:";

    protected IBackofficeStore Backoffice { get; private set; }

    protected bool IsCoreDatabase { get; private set; }


    public BackofficeApi(IBackofficeStore store)
    {
      Store = store.Store;
      Backoffice = store;
      IsCoreDatabase = false;
    }

    internal BackofficeApi(IBackofficeStore store, bool isCoreDatabase)
    {
      Store = store.Store;
      Backoffice = store;
      IsCoreDatabase = isCoreDatabase;
    }


    protected IAsyncDocumentSession Session()
    {
      if (!IsCoreDatabase)
      {
        return Store.OpenAsyncSession();
      }
      else
      {
        return Store.OpenAsyncSession(Backoffice.Options.Raven.Database);
      }
    }


    /// <inheritdoc />
    public async Task<T> GetById<T>(string id) where T : IZeroIdEntity
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      using IAsyncDocumentSession session = Session();
      return await session.LoadAsync<T>(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, T>> GetByIds<T>(params string[] ids) where T : IZeroIdEntity
    {
      using IAsyncDocumentSession session = Session();
      Dictionary<string, T> models = await session.LoadAsync<T>(ids);
      Dictionary<string, T> result = new Dictionary<string, T>();

      foreach (string id in ids)
      {
        models.TryGetValue(id, out T model);
        result.Add(id, model);
      }

      return result;
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> SaveModel<T>(T model, IValidator<T> validator = null, Action<IMetadataDictionary> meta = null) where T : IZeroEntity
    {
      // check for alias
      //if (model is IUrlAliasEntity)
      //{
      //  IUrlAliasEntity entity = operation.Model as IUrlAliasEntity;
      //  entity.Alias = entity.Alias?.ToLower().ToUrlSegment();
      //}

      // run validator
      if (validator != null)
      {
        ValidationResult validation = await validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
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
          item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? IdGenerator.Create(item.Item.Length.Value) : IdGenerator.Create());
        }
      }

      // get current user
      string userId = Backoffice.Auth.GetUserId();

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        model.CreatedDate = DateTimeOffset.Now;
        model.CreatedById = userId;

        if (model is ILanguageAwareEntity)
        {
          (model as ILanguageAwareEntity).LanguageId = "languages.1-A"; // TODO correct language id
        }
      }

      // update name alias and last modified
      model.Alias = Safenames.Alias(model.Name);
      model.LastModifiedById = userId;
      model.LastModifiedDate = DateTimeOffset.Now;
      model.CreatedById ??= userId;
      model.Hash ??= IdGenerator.Classic();

      using IAsyncDocumentSession session = Session();
      session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

      await session.StoreAsync(model);

      meta?.Invoke(session.Advanced.GetMetadataFor(model));

      await session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> DeleteById<T>(string id) where T : IZeroIdEntity
    {
      using IAsyncDocumentSession session = Session();
      session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

      T entity = await session.LoadAsync<T>(id);

      if (entity == null)
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
      string database = IsCoreDatabase ? Backoffice.Options.Raven.Database : null;
      await Store.PurgeAsync<T>(database, querySuffix, parameters);
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
    Task<EntityResult<T>> SaveModel<T>(T model, IValidator<T> validator = null, Action<IMetadataDictionary> meta = null) where T : IZeroEntity;

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
}
