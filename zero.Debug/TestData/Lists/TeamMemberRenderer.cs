using FluentValidation;
using zero.Core.Renderer;

namespace zero.TestData.Lists
{
  public class TeamMemberRenderer : AbstractRenderer<TeamMember>
  {
    public TeamMemberRenderer()
    {
      LabelTemplate = "@_test.fields.{0}";
      DescriptionTemplate = "@_test.fields.{0}_text";

      Validator = new TeamMemberValidator();

      Field(x => x.Name, required: true).Text(opts => opts.Placeholder = "Enter your name");
      Field(x => x.Position, required: true).Text();
      Field(x => x.Image).Media(opts => opts.Type = MediaOptionsType.Image);
      Field(x => x.Email, required: true).Text(opts => opts.Classes.Add("email-field"));
      Field(x => x.VideoUri).Text();
      //Field(x => x.Addresses, required: true).Nested(new AddressRenderer(), opts =>
      //{
      //  opts.Limit = 5;
      //  opts.AddLabel = "Add address";
      //});
    }
  }


  public class AddressRenderer : AbstractRenderer<TeamMemberAddress>
  {
    public AddressRenderer()
    {
      LabelTemplate = "@_test.fields.address.{0}";
      DescriptionTemplate = "@{0}";

      Field(x => x.City, required: true).Text();
      Field(x => x.Street).Text();
      Field(x => x.No).Text(opts => opts.Classes.Add("is-short"));
      //Field(x => x.CountryId).Custom("plugins/countryPicker/countrypicker", () => new
      //{
      //  startId = 107
      //});
    }
  }


  public class TeamMemberValidator : AbstractValidator<TeamMember>
  {
    public TeamMemberValidator()
    {
      RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
      RuleFor(x => x.Position).NotEmpty().Length(3, 60);
      RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
  }
}