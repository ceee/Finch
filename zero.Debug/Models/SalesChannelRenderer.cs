using zero.Commerce.Entities;
using zero.Core.Renderer;
using zero.Debug.Models;

namespace zero.Commerce.Backoffice
{
  public class SalesChannelRenderer : AbstractRenderer<SalesChannel>
  {
    public SalesChannelRenderer() : base()
    {

      Field(x => x.Name, required: false).Text();

      //Field(x => x.Name, "@ui.name", required: true, noDescription: true).Text();

      //Field(x => x.CheckoutType).State(opts =>
      //{
      //  opts.Add("@channel.checkout_states.none", ChannelCheckoutType.None);
      //  opts.Add("@channel.checkout_states.order", ChannelCheckoutType.Order);
      //  opts.Add("@channel.checkout_states.request", ChannelCheckoutType.Request);
      //  opts.Add("@channel.checkout_states.both", ChannelCheckoutType.All);
      //});
      //Field(x => x.ProductSorting).State(opts =>
      //{
      //  opts.Add("@channel.sorting_states.relevance", ChannelProductSorting.Relevance);
      //  opts.Add("@channel.sorting_states.new", ChannelProductSorting.New);
      //  opts.Add("@channel.sorting_states.price", ChannelProductSorting.Price);
      //});

      //Tab("@channel.tabs.theme", () =>
      //{
      //  Field(x => x.Theme.ImageId, required: true).Text();
      //});

      //Tab("@channel.tabs.settings", () =>
      //{
      //  Field(x => x.ClientNo).Text();
      //  Field(x => x.SortLetter).Text(opts =>
      //  {
      //    opts.Classes.Add("is-short");
      //    opts.MaxLength = 1;
      //  });
      //});
    }
  }
}
