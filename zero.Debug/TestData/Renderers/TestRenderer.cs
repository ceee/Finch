using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using zero.Core.Renderer;

namespace zero.TestData
{
  public class TestRenderer : AbstractRenderer<RedirectPage>
  {
    public TestRenderer()
    {
      Alias = "debug.redirectpage";

      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Field(x => x.Name, required: true).Text();

      Field(x => x.Name, required: true).Text(opts => opts.Placeholder = "Enter your name");
      Field(x => x.Link, required: true).Text();

      Tab("Options", () =>
      {
        Field(x => x.Options).Renderer(new OptionsPagePartialRenderer(), opts => opts.HideLabel = true);
      });
    }
  }
}
