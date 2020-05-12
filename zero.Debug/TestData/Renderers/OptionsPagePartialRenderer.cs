using FluentValidation;
using zero.Core.Renderer;

namespace zero.TestData
{
  public class OptionsPagePartialRenderer : AbstractRenderer<OptionsPagePartial>
  {
    public OptionsPagePartialRenderer()
    {
      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Validator = new OptionsPagePartialValidator();

      Field(x => x.HideInNavigation).Toggle();
    }
  }


  public class OptionsPagePartialValidator : AbstractValidator<OptionsPagePartial>
  {
    public OptionsPagePartialValidator()
    {
      
    }
  }
}
