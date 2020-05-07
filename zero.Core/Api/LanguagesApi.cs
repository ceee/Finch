using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class LanguagesApi : ILanguagesApi
  {
    protected IDocumentStore Raven { get; private set; }


    public LanguagesApi(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public async Task<Language> GetById(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<Language>(id);
      }
    }


    /// <inheritdoc />
    public async Task<IList<Language>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Language>()
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public IList<Culture> GetAllCultures(params string[] codes)
    {
      return CultureInfo.GetCultures(CultureTypes.AllCultures)
        .Where(x => !x.Name.IsNullOrWhiteSpace())
        .Select(x => new CultureInfo(x.Name))
        .Where(x => codes.Length > 0 ? codes.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase) : true)
        .OrderBy(x => x.DisplayName)
        .Select(x => new Culture()
        {
          Code = x.Name,
          Name = x.DisplayName
        })
        .ToList();
    }


    /// <inheritdoc />
    public async Task<ListResult<Language>> GetByQuery(ListQuery<Language> query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Language>()
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Language>> Save(Language model)
    {
      ValidationResult validation = await new LanguageValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<Language>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = Constants.Database.SharedAppId;
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Alias.Generate(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);
        await session.SaveChangesAsync();
      }

      return EntityResult<Language>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Language>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        Language entity = await session.LoadAsync<Language>(id);

        if (entity == null)
        {
          return EntityResult<Language>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(entity);

        await session.SaveChangesAsync();
      }

      return EntityResult<Language>.Success();
    }
  }


  public interface ILanguagesApi
  {
    /// <summary>
    /// Get language by Id
    /// </summary>
    Task<Language> GetById(string id);

    /// <summary>
    /// Get all available languages
    /// </summary>
    Task<IList<Language>> GetAll();

    /// <summary>
    /// Get all available cultures
    /// </summary>
    IList<Culture> GetAllCultures(params string[] codes);

    /// <summary>
    /// Get all available languages (with query)
    /// </summary>
    Task<ListResult<Language>> GetByQuery(ListQuery<Language> query);

    /// <summary>
    /// Creates or updates a language
    /// </summary>
    Task<EntityResult<Language>> Save(Language model);

    /// <summary>
    /// Deletes a language by Id
    /// </summary>
    Task<EntityResult<Language>> Delete(string id);
  }
}
