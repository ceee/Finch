using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.TestData;

namespace zero.Debug.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet]
    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> SavePages([FromServices] IDocumentStore raven)
    {
      using (IAsyncDocumentSession session = raven.OpenAsyncSession())
      {
        Application app = await session.Query<Application>().Where(x => x.IsActive).FirstOrDefaultAsync();

        await session.StoreAsync(new ContentPage()
        {
          CreatedDate = DateTimeOffset.Now,
          Name = "Home",
          Alias = "home",
          IsActive = true,
          PageTypeAlias = "root",
          Text = "This is a text",
          AppId = app.Id,
          Meta = new MetaPagePartial()
          {
            TitleOverrideAll = "brothers Klika OG"
          },
          Options = new OptionsPagePartial()
          {
            HideInNavigation = true
          }
        });

        await session.StoreAsync(new ContentPage()
        {
          CreatedDate = DateTimeOffset.Now,
          Name = "Products",
          Alias = "products",
          IsActive = true,
          PageTypeAlias = "content",
          Text = "Our products page",
          AppId = app.Id
        });

        ContentPage newsPage = new ContentPage()
        {
          CreatedDate = DateTimeOffset.Now,
          Name = "News",
          Alias = "news",
          IsActive = true,
          PageTypeAlias = "content",
          Text = "1 out of 100 news are good",
          AppId = app.Id
        };

        await session.StoreAsync(newsPage);

        await session.StoreAsync(new NewsPage()
        {
          CreatedDate = DateTimeOffset.Now,
          Name = "Our new website is online",
          Alias = "xxxx",
          IsActive = true,
          PageTypeAlias = "news",
          Text = "What the fuckk",
          PublishDate = DateTimeOffset.Now.AddDays(3),
          AppId = app.Id,
          ParentId = newsPage.Id.Ref<IPage>()
        });

        await session.StoreAsync(new NewsPage()
        {
          CreatedDate = DateTimeOffset.Now,
          Name = "This is a test news",
          Alias = "xxx",
          IsActive = false,
          PageTypeAlias = "news",
          Text = "What the fuckkii",
          PublishDate = DateTimeOffset.Now.AddDays(-20),
          AppId = app.Id,
          ParentId = newsPage.Id.Ref<IPage>()
        });

        await session.StoreAsync(new RedirectPage()
        {
          CreatedDate = DateTimeOffset.Now,
          Name = "Visit our website",
          Alias = "xxx",
          IsActive = true,
          PageTypeAlias = "redirect",
          AppId = app.Id,
          ParentId = newsPage.Id.Ref<IPage>(),
          Link = "https://brothers.studio"
        });

        await session.SaveChangesAsync();
      }

      return Ok();
    }
  }
}