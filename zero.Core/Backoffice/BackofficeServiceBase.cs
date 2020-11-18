using FluentValidation;
using FluentValidation.Results;
using Raven.Client;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Attributes;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Core.Backoffice
{
  public abstract class BackofficeService<T> : IBackofficeService<T> where T : IZeroEntity
  {
    /// <inheritdoc />
    public string Database { get; set; }

    /// <summary>
    /// Zero context
    /// </summary>
    protected readonly IZeroContext Context;

    /// <summary>
    /// Document store
    /// </summary>
    protected readonly IZeroStore Store;

    /// <summary>
    /// The validator
    /// </summary>
    protected readonly IValidator<T> Validator;


    public BackofficeService(IZeroContext context, IValidator<T> validator = null)
    {
      Context = context;
      Store = context.Store;
      Validator = validator;
    }


    /// <summary>
    /// Create an an async document session
    /// </summary>
    protected virtual IAsyncDocumentSession Session()
    {
      return Database.IsNullOrWhiteSpace() ? Store.OpenAsyncSession() : Store.OpenAsyncSession(Database);
    }


    /// <inheritdoc />
    public virtual async Task<T> GetById(string id)
    {
      if (id.IsNullOrWhiteSpace())
      {
        return default;
      }

      using IAsyncDocumentSession session = Session();
      return await session.LoadAsync<T>(id);
    }


    /// <inheritdoc />
    public virtual async Task<Dictionary<string, T>> GetByIds(params string[] ids)
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
    public virtual async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      using IAsyncDocumentSession session = Session();
      return await session.Query<T>().ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public virtual IAsyncEnumerable<T> Stream() => Stream(null);


    /// <inheritdoc />
    public virtual async IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression)
    {
      using IAsyncDocumentSession session = Session();
      IRavenQueryable<T> query = session.Query<T>();

      if (expression != null)
      {
        query = expression(query);
      }

      var stream = await session.Advanced.StreamAsync(query);

      while (await stream.MoveNextAsync())
      {
        yield return stream.Current.Document;
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      bool isCreate = false;

      // run validator
      if (Validator != null)
      {
        ValidationResult validation = await Validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      // find all Raven Ids
      List<ObjectTraverser.Result<GenerateIdAttribute>> ravenIds = ObjectTraverser.FindAttribute<GenerateIdAttribute>(model);

      // set unset Raven Ids
      foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ravenIds)
      {
        string id = item.Property.GetValue(item.Parent, null) as string;
        if (String.IsNullOrWhiteSpace(id))
        {
          item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? IdGenerator.Create(item.Item.Length.Value) : IdGenerator.Create());
        }
      }

      // get current user
      string userId = Context.BackofficeUser.FindFirstValue(Constants.Auth.Claims.UserId);

      // set default properties
      if (model.Id.IsNullOrEmpty())
      {
        isCreate = true;

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
      model.Hash ??= IdGenerator.Create();

      using IAsyncDocumentSession session = Session();
      session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

      await session.StoreAsync(model);

      //await Backoffice.Messages.Publish(new EntitySavedMessage<T>()
      //{
      //  Id = model.Id,
      //  IsCreate = isCreate,
      //  IsDelete = false,
      //  Model = model,
      //  Session = session
      //});

      await session.SaveChangesAsync();

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> Delete(T model) => await DeleteById(model?.Id);


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> DeleteById(string id)
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
    public virtual async Task<int> Delete(params T[] models) => await DeleteByIds(models.Select(x => x.Id).ToArray());


    /// <inheritdoc />
    public virtual async Task<int> DeleteByIds(params string[] ids)
    {
      int successCount = 0;

      foreach (string id in ids)
      {
        EntityResult<T> result = await DeleteById(id);
        successCount += result.IsSuccess ? 1 : 0;
      }

      return successCount;
    }


    /// <inheritdoc />
    public virtual async Task<EntityResult<T>> Purge(string querySuffix = null, Parameters parameters = null)
    {
      await Store.PurgeAsync<T>(Database, querySuffix, parameters);
      return EntityResult<T>.Success();
    }
  }


  public interface IBackofficeService<T> where T : IZeroEntity
  {
    /// <summary>
    /// The database to operate on.
    /// Is null by default, which uses the database from the resolved application.
    /// </summary>
    string Database { get; set; }

    /// <summary>
    /// Get an entity by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get entities by ids
    /// </summary>
    Task<Dictionary<string, T>> GetByIds(params string[] ids);

    /// <summary>
    /// Get entities by query
    /// </summary>
    Task<ListResult<T>> GetByQuery(ListQuery<T> query);

    /// <summary>
    /// Stream the collection
    /// </summary>
    IAsyncEnumerable<T> Stream();

    /// <summary>
    /// Stream the collection
    /// </summary>
    IAsyncEnumerable<T> Stream(Func<IRavenQueryable<T>, IRavenQueryable<T>> expression);

    /// <summary>
    /// Updates or creates an entity with an optional validator
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes an entity
    /// </summary>
    Task<EntityResult<T>> Delete(T model);

    /// <summary>
    /// Deletes entities
    /// </summary>
    Task<int> Delete(params T[] models);

    /// <summary>
    /// Deletes an entity by Id
    /// </summary>
    Task<EntityResult<T>> DeleteById(string id);

    /// <summary>
    /// Deletes entities by Id
    /// </summary>
    Task<int> DeleteByIds(params string[] ids);

    /// <summary>
    /// Delete a whole collection (with an optional query suffix, i.e. a where statement)
    /// </summary>
    Task<EntityResult<T>> Purge(string querySuffix = null, Parameters parameters = null);
  }
}
