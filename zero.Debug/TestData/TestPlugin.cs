using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using zero.Commerce.Options;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Debug.Sync;
using zero.Debug.TestData;

namespace zero.TestData
{
  public class TestPlugin : IZeroPlugin
  {
    public void Configure(IZeroPluginOptions plugin, IZeroOptions zero)
    {
      plugin.Name = "Test Plugin";
      //ISection spaceSection = zero.Sections.GetAllItems().FirstOrDefault(x => x.Alias == Constants.Sections.Spaces);
      //zero.Sections.Remove(spaceSection);

      zero.Spaces.Add<TeamMemberSpace>();
      zero.Spaces.AddList<News>("news", "News", "Articles about the company", "fth-edit");
      zero.Spaces.AddSeparator();
      zero.Spaces.AddEditor<SocialContent>("social", "Social", "Links to social media", "fth-twitter");

      zero.Features.Add(TestFeatures.Wishlist, "Wishlist", "Frontend wishlist for logged-in users");
      zero.Features.Add(TestFeatures.SocialShopping, "Social shopping", "Integrate products into social media portals");

      zero.Pages.Add<NewsPage>("news", "News", "News about the company", "fth-file-text");
      zero.Pages.Add<ContentPage>("content", "Page", "Page consisting of modules", "fth-box", allowAsRoot: true, allowAllChildrenTypes: true);
      zero.Pages.Add<ContentPage>("root", "Homepage", "Entry point for the website", "fth-home", allowAsRoot: true, allowAllChildrenTypes: true, onlyAtRoot: true);
      zero.Pages.Add<RedirectPage>("redirect", "Redirect", "Redirect to another page or an external URL", "fth-external-link", allowAsRoot: true, allowedChildrenTypes: new List<string>() { "content", "redirect" });

      zero.Modules.Add<RichtextModule>("richtext", "Richtext", "Simple richtext block editor", "fth-align-left", "Texts");
      zero.Modules.Add<HeadlineModule>("headline", "Headline", "Headline with optional subline", "fth-underline", "Texts");
      zero.Modules.Add<TextWithImageModule>("textWithImage", "Text with image", "Short textblock with image", "fth-layers", "Texts", new List<string>() { "root" });
      zero.Modules.Add<GalleryModule>("gallery", "Gallery", "Image gallery grid", "fth-image", "Media");
      zero.Modules.Add<DownloadModule>("download", "Downloads", "List containing downloads", "fth-download", "Misc");
      zero.Modules.Add<OffsetModule>("offset", "Offset", "Offset between two modules", "fth-code", "Misc");
      zero.Modules.Add<NestedModule>("nested", "Nested", "Add nested modules", "fth-layers", "Misc");
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddHostedService<TestPluginStartup>();

      services.Configure<ZeroCommerceOptions>(opts =>
      {
        opts.Documents.Add<InvoiceDocument>();
        opts.Documents.Add<PrintDocument>();
         
        opts.ChannelFeatures.Add("channel.printingMail", "Printing mail", "Send printing mail when order contains labels");
        opts.ChannelFeatures.Add("channel.altFrontend", "Alternative header", "Render a simplified header in the frontend");
      });

      services.AddTransient<CountryBlueprintHandler>(); // TODO auto-register handlers
      services.AddTransient<PropertyBlueprintHandler>();
      services.AddTransient<ITestService, TestService>();
    }
  }
}
