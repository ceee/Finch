using FluentValidation;
using FluentValidation.Results;
using Raven.Client;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Attributes;
using zero.Core.Collections;
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

    protected ICollectionContext Context { get; private set; }

    protected bool IsCoreDatabase { get; private set; }


    public BackofficeApi(ICollectionContext context)
    {
      Context = context;
      Store = context.Store;
      IsCoreDatabase = false;
    }

    internal BackofficeApi(ICollectionContext context, bool isCoreDatabase) : this(context)
    {
      IsCoreDatabase = isCoreDatabase;
    }


    protected IZeroDocumentSession Session => Store.Session(IsCoreDatabase);


    /// <inheritdoc />
    public async Task<T> GetById<T>(string id) where T : ZeroIdEntity
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      return await Session.LoadAsync<T>(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, T>> GetByIds<T>(params string[] ids) where T : ZeroIdEntity
    {
      Dictionary<string, T> models = await Session.LoadAsync<T>(ids);
      Dictionary<string, T> result = new Dictionary<string, T>();

      foreach (string id in ids)
      {
        models.TryGetValue(id, out T model);
        result.Add(id, model);
      }

      return result;
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> SaveModel<T>(T model, IValidator<T> validator = null, Action<IMetadataDictionary> meta = null) where T : ZeroEntity
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
      string userId = Context.Context.BackofficeUser.FindFirstValue(Constants.Auth.Claims.UserId);

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        model.CreatedDate = DateTimeOffset.Now;
        model.CreatedById = userId;
        model.LanguageId = "languages.1-A"; // TODO correct language id
      }

      // update name alias and last modified
      model.Alias = Safenames.Alias(model.Name);
      model.LastModifiedById = userId;
      model.LastModifiedDate = DateTimeOffset.Now;
      model.CreatedById ??= userId;
      model.Hash ??= IdGenerator.Classic();

      Session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

      await Session.StoreAsync(model);

      meta?.Invoke(Session.Advanced.GetMetadataFor(model));

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> DeleteById<T>(string id) where T : ZeroIdEntity
    {
      Session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

      T entity = await Session.LoadAsync<T>(id);

      if (entity == null)
      {
        return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
      }

      Session.Delete(entity);

      await Session.SaveChangesAsync();

      return EntityResult<T>.Success();
    }


    /// <inheritdoc />
    public async Task<int> DeleteByIds<T>(params string[] ids) where T : ZeroIdEntity
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
      await Store.PurgeAsync<T>(Session.Advanced.DocumentStore.Database, querySuffix, parameters);
      return EntityResult<T>.Success();
    }
  }


  public interface IBackofficeApi
  {
    /// <summary>
    /// Get an entity by Id.
    /// If the requested entity is an IAppAwareEntity it will only return entities for the currently selected app + shared app
    /// </summary>
    Task<T> GetById<T>(string id) where T : ZeroIdEntity;

    /// <summary>
    /// Get entities by ids.
    /// If the requested entity is an IAppAwareEntity it will only return entities for the currently selected app + shared app
    /// </summary>
    Task<Dictionary<string, T>> GetByIds<T>(params string[] ids) where T : ZeroIdEntity;

    /// <summary>
    /// Updates or creates an entity with an optional validator
    /// </summary>
    Task<EntityResult<T>> SaveModel<T>(T model, IValidator<T> validator = null, Action<IMetadataDictionary> meta = null) where T : ZeroEntity;

    /// <summary>
    /// Deletes an entity by Id
    /// </summary>
    Task<EntityResult<T>> DeleteById<T>(string id) where T : ZeroIdEntity;

    /// <summary>
    /// Deletes entities by Id
    /// </summary>
    Task<int> DeleteByIds<T>(params string[] ids) where T : ZeroIdEntity;

    /// <summary>
    /// Delete a whole collection (with an optional query suffix, i.e. a where statement)
    /// </summary>
    Task<EntityResult<T>> Purge<T>(string querySuffix = null, Parameters parameters = null);
  }
}
