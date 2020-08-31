//using FluentValidation;
//using FluentValidation.Results;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Session;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using zero.Core.Entities;
//using zero.Core.Extensions;

//namespace zero.Core.Api
//{
//  public class CollectionHistoryApi : AppAwareBackofficeApi, ICollectionHistoryApi
//  {
//    public CollectionHistoryApi(IBackofficeStore store) : base(store)
//    {
//      Scope.IncludeShared = true;
//    }


//    /// <inheritdoc />
//    public async Task<T> GetLastEditedEntityBy<T>(string userId) where T : IZeroEntity
//    {
//      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

//      return await session.Query<T>()
//        .Scope(Scope)
//        .OrderByDescending(x => x.l)
//        .ThenBy(x => x.Name)
//        .ToListAsync();
//    }


//    /// <inheritdoc />
//    public async Task<IList<ICountry>> GetAll(string languageId)
//    {
//      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
//      {
//        return await session.Query<ICountry>()
//          .Scope(Scope)
//          .Where(x => x.LanguageId == languageId)
//          .OrderByDescending(x => x.IsPreferred)
//          .ThenBy(x => x.Name)
//          .ToListAsync();
//      }
//    }


//    /// <inheritdoc />
//    public async Task<ListResult<ICountry>> GetByQuery(string languageId, ListQuery<ICountry> query)
//    {
//      query.SearchSelector = country => country.Name;

//      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
//      {
//        return await session.Query<ICountry>()
//          .Scope(Scope)
//          .Where(x => x.LanguageId == languageId)
//          .OrderByDescending(x => x.IsPreferred)
//          .ThenBy(x => x.Name)
//          .ToQueriedListAsync(query);
//      }
//    }
//  }


//  public interface ICollectionHistoryApi
//  {
//    /// <summary>
//    /// Get country by Id
//    /// </summary>
//    Task<ICountry> GetById(string id);

//    /// <summary>
//    /// Get countries by ids
//    /// </summary>
//    Task<Dictionary<string, ICountry>> GetByIds(params string[] ids);

//    /// <summary>
//    /// Get all available countries
//    /// </summary>
//    Task<IList<ICountry>> GetAll(string languageId);

//    /// <summary>
//    /// Get all available countries (with query)
//    /// </summary>
//    Task<ListResult<ICountry>> GetByQuery(string languageId, ListQuery<ICountry> query);

//    /// <summary>
//    /// Creates or updates a country
//    /// </summary>
//    Task<EntityResult<ICountry>> Save(ICountry model);

//    /// <summary>
//    /// Deletes a country by Id
//    /// </summary>
//    Task<EntityResult<ICountry>> Delete(string id);
//  }
//}
