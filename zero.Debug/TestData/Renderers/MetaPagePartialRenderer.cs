using FluentValidation;
using zero.Core.Renderer;

namespace zero.TestData
{
  public class MetaPagePartialRenderer : AbstractRenderer<MetaPagePartial>
  {
    public MetaPagePartialRenderer()
    {
      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Validator = new MetaPagePartialValidator();

      Field(x => x.HideInTitle).Toggle();
      Field(x => x.TitleOverride).Text();
      Field(x => x.TitleOverrideAll).Text();
      Field(x => x.SeoDescription).Textarea(opts => opts.MaxLength = 160);
      Field(x => x.SeoImage).Media();
      Field(x => x.NoFollow).Toggle();
      Field(x => x.NoIndex).Toggle();
    }
  }


  public class MetaPagePartialValidator : AbstractValidator<MetaPagePartial>
  {
    public MetaPagePartialValidator()
    {
      RuleFor(x => x.SeoDescription).MaximumLength(160);
    }
  }
}
