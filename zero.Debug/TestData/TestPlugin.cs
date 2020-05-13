using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Debug.TestData;
using zero.TestData.Lists;

namespace zero.TestData
{
  public class TestPlugin : IZeroPlugin
  {
    public void Configure(IServiceCollection services, IZeroPluginConfiguration zero)
    {
      services.AddTransient<ITestService, TestService>();

      zero.Configure<SpaceOptions>(opts =>
      {
        opts.AddList<TeamMember>("team", "Team", "Our team members", "fth-users");
        opts.AddList<News>("news", "News", "Articles about the company", "fth-edit");
        opts.AddSeparator();
        opts.AddEditor<SocialContent>("social", "Social", "Links to social media", "fth-twitter");
      });

      zero.Configure<FeatureOptions>(opts =>
      {
        opts.Add(TestFeatures.Wishlist, "Wishlist", "Frontend wishlist for logged-in users");
        opts.Add(TestFeatures.SocialShopping, "Social shopping", "Integrate products into social media portals");
      });

      zero.Configure<RendererOptions>(opts =>
      {
        opts.Add<TeamMemberRenderer>();
        opts.Add<SocialContentRenderer>();
        opts.Add<OptionsPagePartialRenderer>();
        opts.Add<MetaPagePartialRenderer>();
        opts.Add<NewsPageRenderer>();
        opts.Add<RedirectPageRenderer>();
        opts.Add<ContentPageRenderer>();
      });

      zero.Configure<PageOptions>(opts =>
      {
        opts.Add<NewsPage>("news", "News", "News about the company", "fth-book");
        opts.Add<ContentPage>("content", "Page", "Page consisting of modules", "fth-box", true, true);
        opts.Add<RedirectPage>("redirect", "Redirect", "Redirect to another page or an external URL", "fth-box", true, false, new List<string>() { "content", "redirect" });
      });
    }
  }
}
