using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Plugins;

namespace zero.TestData
{
  public class Test2Plugin : IZeroPlugin
  {
    public void Configure(IServiceCollection services, IZeroPluginBuilder builder)
    {
      builder.PageTypes.Add(new PageType<NewsPage>()
      {
        Alias = "news",
        Name = "News",
        Description = "News about the company",
        Icon = "fth-book"
      });

      builder.PageTypes.Add(new PageType<ContentPage>()
      {
        Alias = "content",
        Name = "Page",
        Description = "Page consisting of modules",
        AllowAsRoot = true,
        AllowAllChildrenTypes = true
      });

      builder.PageTypes.Add(new PageType<RedirectPage>()
      {
        Alias = "redirect",
        Name = "Redirect",
        Description = "Redirect to another page or an external URL",
        AllowedChildrenTypes = new List<string>() { "content", "redirect" }
      });

      builder.Renderers.Add<OptionsPagePartial, OptionsPagePartialRenderer>();
      builder.Renderers.Add<MetaPagePartial, MetaPagePartialRenderer>();
      builder.Renderers.Add<NewsPage, NewsPageRenderer>();
      builder.Renderers.Add<RedirectPage, RedirectPageRenderer>();
      builder.Renderers.Add<ContentPage, ContentPageRenderer>();
    }
  }
}
