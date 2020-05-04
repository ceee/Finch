using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class TranslationsApi : ITranslationsApi
  {
    protected IDocumentStore Raven { get; private set; }


    public TranslationsApi(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public async Task<Translation> GetById(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<Translation>(id);
      }
    }


    /// <inheritdoc />
    public async Task<IList<Translation>> GetAll(string languageId)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Translation>()
          .Where(x => x.LanguageId == languageId)
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Translation>> GetByQuery(string languageId, ListQuery<Translation> query)
    {
      query.SearchSelector = entity => entity.Key;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Translation>()
          .Where(x => x.LanguageId == languageId)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Translation>> Save(Translation model)
    {
      ValidationResult validation = await new TranslationValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<Translation>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = "zero.applications.1-A"; // TODO real app id
        model.CreatedDate = DateTimeOffset.Now;
        model.LanguageId = "en-US"; // TODO
      }

      model.IsActive = true;
      model.Alias = Alias.Generate(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);
        await session.SaveChangesAsync();
      }

      return EntityResult<Translation>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Translation>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        Translation entity = await session.LoadAsync<Translation>(id);

        if (entity == null)
        {
          return EntityResult<Translation>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(entity);

        await session.SaveChangesAsync();
      }

      return EntityResult<Translation>.Success();
    }
  }


  public interface ITranslationsApi
  {
    /// <summary>
    /// Get translation by Id
    /// </summary>
    Task<Translation> GetById(string id);

    /// <summary>
    /// Get all available translations
    /// </summary>
    Task<IList<Translation>> GetAll(string languageId);

    /// <summary>
    /// Get all available translations (with query)
    /// </summary>
    Task<ListResult<Translation>> GetByQuery(string languageId, ListQuery<Translation> query);

    /// <summary>
    /// Creates or updates a translation
    /// </summary>
    Task<EntityResult<Translation>> Save(Translation model);

    /// <summary>
    /// Deletes a translation by Id
    /// </summary>
    Task<EntityResult<Translation>> Delete(string id);
  }
}
