using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Plugins;

namespace zero.TestData
{
  public class Test2Plugin : ZeroPlugin
  {
    public Test2Plugin()
    {
      PageTypes.Add(new PageType<NewsPage>()
      {
        Alias = "news",
        Name = "News",
        Description = "News about the company",
        Icon = "fth-book"
      });

      PageTypes.Add(new PageType<ContentPage>()
      {
        Alias = "content",
        Name = "Page",
        Description = "Page consisting of modules",
        AllowAsRoot = true,
        AllowAllChildrenTypes = true
      });

      PageTypes.Add(new PageType<RedirectPage>()
      {
        Alias = "redirect",
        Name = "Redirect",
        Description = "Redirect to another page or an external URL",
        AllowedChildrenTypes = new List<string>() { "content", "redirect" }
      });

      Renderers.Add<OptionsPagePartial, OptionsPagePartialRenderer>();
      Renderers.Add<MetaPagePartial, MetaPagePartialRenderer>();
      Renderers.Add<NewsPage, NewsPageRenderer>();
      Renderers.Add<RedirectPage, RedirectPageRenderer>();
      Renderers.Add<ContentPage, ContentPageRenderer>();
    }
  }
}
