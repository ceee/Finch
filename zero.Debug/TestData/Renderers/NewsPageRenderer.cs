using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using zero.Core.Renderer;

namespace zero.TestData
{
  public class NewsPageRenderer : AbstractRenderer<NewsPage>
  {
    public NewsPageRenderer()
    {
      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Validator = new NewsPageValidator();

      Field(x => x.Name, required: true).Text(opts => opts.Placeholder = "Enter your name");
      Field(x => x.Text, required: true).Rte();
    }
  }


  public class NewsPageValidator : AbstractValidator<NewsPage>
  {
    public NewsPageValidator()
    {
      RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
      RuleFor(x => x.Text).NotEmpty();
    }
  }
}
