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

      Tab("Networks", () => {

        Field(x => x.Facebook, required: true).Text(opts => opts.Placeholder = "Enter your facebook URL");
        Field(x => x.Youtube).Text();
        Field(x => x.Twitter).Text(opts =>
        {
          opts.Classes.Add("is-short");
        });

      });
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