using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database.Indexes;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class PageUrlResolver : IPageUrlResolver
  {
    const char PATH_SEPARATOR = '/';

    const bool TRAILING_SLASH = false;

    protected IDocumentStore Raven { get; private set; }


    public PageUrlResolver(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(IPage page)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();

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
          stringBuilder.Append(parent.Alias);
          stringBuilder.Append(PATH_SEPARATOR);
        }
      }

      stringBuilder.Append(page.Alias);
      stringBuilder.Append(PATH_SEPARATOR);

      if (!TRAILING_SLASH)
      {
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      }

      return stringBuilder.ToString();
    }


    /// <inheritdoc />
    public UrlInfo GetUrl(IApplicationContext context, IPage page, IEnumerable<IPage> parents)
    {
      StringBuilder stringBuilder = new StringBuilder();

      if (parents != null)
      {
        foreach (IPage parent in parents)
        {
          stringBuilder.Append(parent.Alias);
          stringBuilder.Append(PATH_SEPARATOR);
        }
      }

      stringBuilder.Append(page.Alias);
      stringBuilder.Append(PATH_SEPARATOR);

      if (!TRAILING_SLASH)
      {
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      }

      return new UrlInfo(stringBuilder.ToString(), true, "en-US");
    }
  }
}
