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
using zero.Core.Options;

namespace zero.Core.Routing
{
  public class PageUrlBuilder : IPageUrlBuilder
  {
    const char PATH_SEPARATOR = '/';

    const bool TRAILING_SLASH = false;

    protected IZeroStore Store { get; private set; }

    protected IZeroOptions Options { get; private set; }


    public PageUrlBuilder(IZeroStore store, IZeroOptions options)
    {
      Store = store;
      Options = options;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(Page page)
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      Pages_ByHierarchy.Result result = await session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
          .ProjectInto<Pages_ByHierarchy.Result>()
          .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
          .FirstOrDefaultAsync(x => x.Id == page.Id);

      IList<Page> parents = (await session.LoadAsync<Page>(result.Path.Select(x => x.Id))).Select(x => x.Value).ToList();

      StringBuilder stringBuilder = new StringBuilder();

      if (parents != null)
      {
        foreach (Page parent in parents)
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
    public string GetUrl(Page page, IEnumerable<Page> parents)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(PATH_SEPARATOR);

      void AddPart(Page page)
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
        foreach (Page parent in parents)
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
    protected virtual string GetUrlPart(Page page)
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
      else if (page.PageTypeAlias == Options.Pages.Root)
      {
        return String.Empty;
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
