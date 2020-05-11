using FluentValidation;
using zero.Core.Renderer;

namespace zero.TestData.Lists
{
  public class SocialContentRenderer : AbstractRenderer<SocialContent>
  {
    public SocialContentRenderer()
    {
      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Validator = new SocialContentValidator();

      Field(x => x.IsVisible).Toggle();
      Field(x => x.xIconPicker).IconPicker();
      Field(x => x.xTextarea).Textarea();
      //Field(x => x.Addresses, required: true).Nested(new SocialAddressRenderer(), opts =>
      //{
      //  opts.Limit = 5;
      //  opts.AddLabel = "Add address";
      //});
      Field(x => x.xRte).Rte();
      Field(x => x.xMedia).Media(opts => opts.Type = MediaOptionsType.Image);
      Field(x => x.xTextarea).Output();
      Field(x => x.xState).State(opts =>
      {
        opts.Add("Image", "image");
        opts.Add("Video" , "video");
        opts.Add("Other", "other");
      });
      Field(x => x.xCustom).Custom("MyPlugin/myplugineditor.vue", () => new
      {
        enabled = true,
        name = "tobi"
      });
      Tab("Networks", () => {

        Field(x => x.Facebook, required: true).Text(opts => opts.Placeholder = "Enter your facebook URL");
        Field(x => x.Youtube).Text();
        Field(x => x.Twitter).Text(opts => opts.Classes.Add("is-short"));
      });
    }
  }


  public class SocialAddressRenderer : AbstractRenderer<SocialAddress>
  {
    public SocialAddressRenderer()
    {
      LabelTemplate = "@_test.fields.address.{0}";
      DescriptionTemplate = "@{0}";

      Field(x => x.City, required: true).Text();
      Field(x => x.Street).Text();
      Field(x => x.No).Text(opts => opts.Classes.Add("is-short"));
      Field(x => x.Countries, required: true).Nested(new SocialAddressCountryRenderer(), opts =>
      {
        opts.Limit = 1;
      });
      //Field(x => x.CountryId).Custom("plugins/countryPicker/countrypicker", () => new
      //{
      //  startId = 107
      //});
    }
  }


  public class SocialAddressCountryRenderer : AbstractRenderer<SocialAddressCountry>
  {
    public SocialAddressCountryRenderer()
    {
      LabelTemplate = "@_test.fields.address.{0}";
      DescriptionTemplate = "@{0}";

      Field(x => x.Name, required: true).Text();
      Field(x => x.Iso, required: true).Text();
    }
  }


  public class SocialContentValidator : AbstractValidator<SocialContent>
  {
    public SocialContentValidator()
    {
      RuleFor(x => x.Facebook).NotEmpty().MaximumLength(120);
    }
  }
}