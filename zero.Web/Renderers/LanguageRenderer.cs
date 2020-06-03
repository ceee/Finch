using System;
using zero.Core.Entities;
using zero.Core.Renderer;

namespace zero.Web.Renderers
{
  public class LanguageRenderer : AbstractRenderer<Language>
  {
    const string PREFIX = "@language.fields.";

    public LanguageRenderer()
    {
      FindLabelName = field => PREFIX + field;
      FindLabelDescriptionName = field => PREFIX + field + "_text";

      Field(x => x.Name, description: String.Empty).Text();
      Field(x => x.Code).Text();
      Field(x => x.IsDefault).Toggle();

      //RuleFor(x => x.Name).NotEmpty();
    }
  }
}