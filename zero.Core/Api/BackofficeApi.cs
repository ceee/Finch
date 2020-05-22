using FluentValidation;
using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
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

    IBackofficeStore Backoffice;


    public BackofficeApi(IBackofficeStore store)
    {
      Raven = store.Raven;
      Backoffice = store;
      Scope = new ApiScope()
      {
        Global = true
      };
    }


    /// <inheritdoc />
    public async Task<T> GetById<T>(string id) where T : IZeroIdEntity
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        if (typeof(T).Is<IAppAwareEntity>())
        {
          return await session.Query<T>()
            .Where(x => x.Id == id)
            .Scope(Scope)
            .FirstOrDefaultAsync();
        }

        return await session.LoadAsync<T>(id);
      }
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> Save<T>(T model, IValidator validator = null) where T : IZeroIdEntity
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

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        if (zeroEntity != null)
        {
          zeroEntity.CreatedDate = DateTimeOffset.Now;
        }

        if (appAwareEntity != null && appAwareEntity.AppId.IsNullOrEmpty())
        {
          appAwareEntity.AppId = Scope.AppId; // Constants.Database.SharedAppId; // TODO correct app id
        }

        if (model is ILanguageAwareEntity)
        {
          (model as ILanguageAwareEntity).LanguageId = "languages.1-A"; // TODO correct language id
        }
      }

      // update name alias
      if (zeroEntity != null)
      {
        zeroEntity.Alias = Alias.Generate(zeroEntity.Name);
      }

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        // store entity
        await session.StoreAsync(model);

        // store media

        await session.SaveChangesAsync();
      }

      return EntityResult<T>.Success(model);
    }



    /// <inheritdoc />
    public async Task<EntityResult<T>> DeleteById<T>(string id) where T : IZeroIdEntity
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
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
    /// Updates or creates an entity with an optional validator
    /// </summary>
    Task<EntityResult<T>> Save<T>(T model, IValidator validator = null) where T : IZeroIdEntity;

    /// <summary>
    /// Deletes an entity by Id
    /// </summary>
    Task<EntityResult<T>> DeleteById<T>(string id) where T : IZeroIdEntity;
  }


  public interface IAppAwareBackofficeApi : IBackofficeApi
  {
    ApiScope Scope { get; }
  }
}
