using FluentValidation;
using zero.Core.Entities;
using zero.Core.Renderer;

namespace zero.Web
{
  public class ApplicationRenderer : AbstractRenderer<Application>
  {
    public ApplicationRenderer()
    {
      Alias = "debug.contentpage";

      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Field(x => x.Name, required: true).Text();
    }
  }
}
