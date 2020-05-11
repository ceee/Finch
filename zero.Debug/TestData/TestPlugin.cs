using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Plugins;
using zero.TestData.Lists;

namespace zero.TestData
{
  public class TestPlugin : ZeroPlugin
  {
    public TestPlugin()
    {
      Spaces.AddList<TeamMember>("team", "Team", "Our team members", "fth-users");
      Spaces.AddList<News>("news", "News", "Articles about the company", "fth-edit");
      Spaces.AddSeparator();
      Spaces.AddEditor<SocialContent>("social", "Social", "Links to social media", "fth-twitter");

      Features.Add(TestFeatures.Wishlist, "Wishlist", "Frontend wishlist for logged-in users");
      Features.Add(TestFeatures.SocialShopping, "Social shopping", "Integrate products into social media portals");

      Renderers.Add<TeamMember, TeamMemberRenderer>();
      Renderers.Add<SocialContent, SocialContentRenderer>();
    }
  }
}
