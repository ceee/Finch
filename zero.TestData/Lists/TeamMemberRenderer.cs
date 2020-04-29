using FluentValidation;
using zero.Core.Renderer;

namespace zero.TestData.Lists
{
  public class TeamMemberRenderer : AbstractRenderer<TeamMember>
  {
    public TeamMemberRenderer()
    {
      LabelTemplate = "@team.fields.{0}";
      DescriptionTemplate = "@team.fields.{0}_text";

      Validator = new TeamMemberValidator();

      Field(x => x.Name, required: true).Text();

      Tab("@team.tab.extras", () =>
      {
        Field(x => x.Position, required: true).Text();
        Field(x => x.Image).Media(opts =>
        {
          opts.Type = MediaOptionsType.Image;
        });
        Field(x => x.Email, required: true).Text();
        Field(x => x.VideoUri).Text();
      });

      Tab("@team.tab.permissions", () =>
      {
        Box("Acess to sections", null, () =>
        {
          Field(x => x.Alias).Toggle();
          Field(x => x.AppId).Toggle();
          Field(x => x.CreatedDate).State(opts =>
          {
            opts.Add("None", "none");
            opts.Add("View", "view");
            opts.Add("Edit", "edit");
          });
        });
      });
    }
  }


  public class TeamMemberValidator : AbstractValidator<TeamMember>
  {
    public TeamMemberValidator()
    {
      
    }
  }
}
