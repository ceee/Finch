using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Routing
{
  public class PageUrlBuilder : IPageUrlBuilder
  {
    const char PATH_SEPARATOR = '/';

    const bool TRAILING_SLASH = false;

    protected IZeroStore Store { get; private set; }


    public PageUrlBuilder(IZeroStore store)
    {
      Store = store;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(IPage page)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      Pages_ByHierarchy.Result result = await session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
          .ProjectInto<Pages_ByHierarchy.Result>()
          .Include<Pages_ByHierarchy.Result, IPage>(x => x.Path.Select(p => p.Id))
          .FirstOrDefaultAsync(x => x.Id == page.Id);

      IList<IPage> parents = (await session.LoadAsync<IPage>(result.Path.Select(x => x.Id))).Select(x => x.Value).ToList();

      StringBuilder stringBuilder = new StringBuilder();

      if (parents != null)
      {
        foreach (IPage parent in parents)
        {
          stringBuilder.Append(GetUrlPart(parent));
          stringBuilder.Append(PATH_SEPARATOR);
        }
      }

      stringBuilder.Append(GetUrlPart(page));
      stringBuilder.Append(PATH_SEPARATOR);

      if (!TRAILING_SLASH)
      {
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      }

      return stringBuilder.ToString();
    }


    /// <inheritdoc />
    public string GetUrl(IPage page, IEnumerable<IPage> parents)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(PATH_SEPARATOR);

      void AddPart(IPage page)
      {
        string part = GetUrlPart(page);

        if (!part.IsNullOrEmpty())
        {
          stringBuilder.Append(part);
          stringBuilder.Append(PATH_SEPARATOR);
        }
      }

      if (parents != null)
      {
        foreach (IPage parent in parents)
        {
          AddPart(parent);
        }
      }

      AddPart(page);

      if (!TRAILING_SLASH && stringBuilder.Length > 1)
      {
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      }

      return stringBuilder.ToString();
    }


    /// <summary>
    /// Get the part of the URL (by querying UrlAlias and Alias) for this page
    /// </summary>
    protected virtual string GetUrlPart(IPage page)
    {
      string alias;

      if (page is PageFolder && !((PageFolder)page).IsPartOfRoute)
      {
        return null;
      }

      if (!page.UrlAlias.IsNullOrWhiteSpace())
      {
        alias = page.UrlAlias;
      }
      else if (!page.Alias.IsNullOrWhiteSpace())
      {
        alias = page.Alias;
      }
      else
      {
        alias = page.Name;
      }

      return Safenames.Alias(alias).Trim().Trim(PATH_SEPARATOR);
    }
  }
}
