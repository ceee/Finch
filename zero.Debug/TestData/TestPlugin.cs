using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Debug;
using zero.Debug.TestData;
using zero.TestData.Lists;

namespace zero.TestData
{
  public class TestPlugin : IZeroPlugin
  {
    public void Configure(IZeroPluginOptions plugin, IZeroOptions zero)
    {
      plugin.Name = "Test Plugin";

      zero.Spaces.AddList<TeamMember>("team", "Team", "Our team members", "fth-users");
      zero.Spaces.AddList<News>("news", "News", "Articles about the company", "fth-edit");
      zero.Spaces.AddSeparator();
      zero.Spaces.AddEditor<SocialContent>("social", "Social", "Links to social media", "fth-twitter");

      zero.Features.Add(TestFeatures.Wishlist, "Wishlist", "Frontend wishlist for logged-in users");
      zero.Features.Add(TestFeatures.SocialShopping, "Social shopping", "Integrate products into social media portals");

      zero.Renderers.Add<TeamMemberRenderer>();
      zero.Renderers.Add<SocialContentRenderer>();
      zero.Renderers.Add<OptionsPagePartialRenderer>();
      zero.Renderers.Add<MetaPagePartialRenderer>();
      zero.Renderers.Add<NewsPageRenderer>();
      zero.Renderers.Add<RedirectPageRenderer>();
      zero.Renderers.Add<ContentPageRenderer>();

      zero.Pages.Add<NewsPage>("news", "News", "News about the company", "fth-file-text");
      zero.Pages.Add<ContentPage>("content", "Page", "Page consisting of modules", "fth-box", allowAsRoot: true, allowAllChildrenTypes: true);
      zero.Pages.Add<ContentPage>("root", "Homepage", "Entry point for the website", "fth-home", allowAsRoot: true, allowAllChildrenTypes: true);
      zero.Pages.Add<RedirectPage>("redirect", "Redirect", "Redirect to another page or an external URL", "fth-external-link", allowAsRoot: true, allowedChildrenTypes: new List<string>() { "content", "redirect" });
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.Replace<ILanguage, MyLanguage>();

      services.AddTransient<ITestService, TestService>();
    }
  }
}
