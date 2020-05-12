using FluentValidation;
using zero.Core.Renderer;

namespace zero.TestData
{
  public class ContentPageRenderer : AbstractRenderer<ContentPage>
  {
    public ContentPageRenderer()
    {
      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Validator = new ContentPageValidator();

      Field(x => x.Name, required: true).Text();

      Tab("Options", () =>
      {
        Field(x => x.Options).Renderer(new OptionsPagePartialRenderer());
      });

      Tab("Meta", () =>
      {
        Field(x => x.Meta).Renderer(new MetaPagePartialRenderer());
      });
    }
  }


  public class ContentPageValidator : AbstractValidator<ContentPage>
  {
    public ContentPageValidator()
    {
      RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
    }
  }
}
