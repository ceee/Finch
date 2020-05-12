using Microsoft.Extensions.DependencyInjection;
using zero.Core.Plugins;
using zero.TestData.Lists;

namespace zero.TestData
{
  public class TestPlugin : IZeroPlugin
  {
    public void Configure(IServiceCollection services, IZeroPluginBuilder builder)
    {
      builder.Spaces.AddList<TeamMember>("team", "Team", "Our team members", "fth-users");
      builder.Spaces.AddList<News>("news", "News", "Articles about the company", "fth-edit");
      builder.Spaces.AddSeparator();
      builder.Spaces.AddEditor<SocialContent>("social", "Social", "Links to social media", "fth-twitter");

      builder.Features.Add(TestFeatures.Wishlist, "Wishlist", "Frontend wishlist for logged-in users");
      builder.Features.Add(TestFeatures.SocialShopping, "Social shopping", "Integrate products into social media portals");

      builder.Renderers.Add<TeamMember, TeamMemberRenderer>();
      builder.Renderers.Add<SocialContent, SocialContentRenderer>();
    }
  }
}
