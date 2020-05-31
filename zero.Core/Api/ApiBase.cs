using FluentValidation;
using FluentValidation.Results;
using Raven.Client.Documents;
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
  public abstract class ApiBase
  {
    protected IDocumentStore Raven { get; set; }

    protected IMediaUpload Media { get; set; }

    protected const string ASTERISK = "*";

    protected const string NEW_ID = "new:";


    public ApiBase(IDocumentStore raven, IMediaUpload media)
    {
      Raven = raven;
      Media = media;
    }


    /// <summary>
    /// Get an entity by Id
    /// </summary>
    protected async Task<T> GetById<T>(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        // TODO store-aware?
        return await session.LoadAsync<T>(id);
      }
    }



    /// <summary>
    /// Get an entity by Id and transform the result
    /// </summary>
    protected async Task<EntityResult<T>> Save<T>(T model, IValidator<T> validator = null) where T : IZeroEntity
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
      List<ObjectTraverser.Result<Media>> media = ObjectTraverser.Find<Media>(model);

      // upload media items
      Dictionary<string, Media> mediaItems = new Dictionary<string, Media>();

      foreach (ObjectTraverser.Result<Media> item in media)
      {
        string id = item.Item?.Id;

        if (!Media.Upload(item.Item, out bool uploaded, out string uploadError))
        {
          return EntityResult<T>.Fail(item.Path, uploadError);
        }
        else
        {
          mediaItems.Add(id, item.Item);
        }
      }

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
        model.CreatedDate = DateTimeOffset.Now;

        if (model is IAppAwareEntity)
        {
          (model as IAppAwareEntity).AppId = Constants.Database.SharedAppId; // TODO correct app id
        }

        if (model is ILanguageAwareEntity)
        {
          (model as ILanguageAwareEntity).LanguageId = "zero.languages.1-A"; // TODO correct language id
        }
      }

      // update name alias
      model.Alias = Safenames.Alias(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);
        await session.SaveChangesAsync();
      }

      return EntityResult<T>.Success(model);
    }


    /// <summary>
    /// Deletes an entity by Id
    /// </summary>
    protected async Task<EntityResult<T>> DeleteById<T>(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        T entity = await session.LoadAsync<T>(id);

        // TODO || !Store.Has(entity))
        if (entity == null)
        {
          return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(entity);

        await session.SaveChangesAsync();

        return EntityResult<T>.Success();
      }
    }
  }
}