using FluentValidation;
using zero.Core.Entities;
using zero.Core.Renderer;

namespace zero.Web.Renderers
{
  public class ApplicationRenderer : AbstractRenderer<Application>
  {
    const string PREFIX = "@application.fields.";


    public ApplicationRenderer()
    {
      Alias = "application";

      FindLabelName = field => PREFIX + field;
      FindLabelDescriptionName = field => PREFIX + field + "_text";

      Field(x => x.Name, true).Text();
      Field(x => x.FullName).Text();
      Field(x => x.Email, true).Text();
      Field(x => x.ImageId).Media(opts => opts.Type = MediaOptionsType.Image);
      Field(x => x.IconId).Media(opts => opts.Type = MediaOptionsType.Image);

      Tab("@application.tab_domains", () =>
      {
        Field(x => x.Domains).TextList(opts =>
        {
          opts.Limit = 10;
          opts.AddLabel = PREFIX + "domains_add";
          opts.HelpText = PREFIX + "domains_help";
        });
      });

      Tab("@application.tab_features", () =>
      {

      });
      //RuleFor(x => x.Name).NotEmpty();
    }
  }


  //public class ApplicationValidator : AbstractValidator<Application>
  //{
  //  public ApplicationValidator()
  //  {
  //    RuleFor(x => x.Name).NotNull()
  //  }
  //}
}