using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Identity;
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

      var commercePermissions = new PermissionCollection()
      {
        Label = "Commerce"
      };

      commercePermissions.Items.Add(new Permission("commerce.orders", "Orders", "Manage and fulfill orders", PermissionValueType.ReadWrite));
      commercePermissions.Items.Add(new Permission("commerce.channels", "Channels", "Create and manage sales channels", PermissionValueType.ReadWrite));
      commercePermissions.Items.Add(new Permission("commerce.newchannels", "Create channels", "Create new channels", PermissionValueType.Boolean));

      Permissions.Add(commercePermissions);

      Section section = new Section("commerce", "Commerce", "fth-shopping-bag", "#52bba1");
      section.Children.Add(new Section("orders", "Orders"));
      section.Children.Add(new Section("customers", "Customers"));
      section.Children.Add(new Section("catalogue & ÖBB", "Catalogue"));
      section.Children.Add(new Section("promotions", "Promotions"));
      section.Children.Add(new Section("channels", "Channels"));

      Sections.Insert(3, section);
    }
  }


  public abstract class ZeroPlugin
  {
    protected IServiceCollection Services { get; private set; }

    protected SectionCollection Sections { get; private set; }

    protected SpaceCollection Spaces { get; private set; }

    protected RendererCollection Renderers { get; private set; }

    protected IList<SettingsGroup> Settings { get; private set; }

    public IList<PermissionCollection> Permissions { get; private set; }

    protected FeatureCollection Features { get; private set; }
  }




  public interface IZeroPlugin
  {
  }
}
