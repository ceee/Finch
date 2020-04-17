using zero.Core.Entities;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class UserMapper : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      config.CreateMap<User, UserEditModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.CreatedDate = source.CreatedDate;
        target.IsSuper = source.IsSuper;
        target.Email = source.Email;
        target.IsEmailConfirmed = source.IsEmailConfirmed;
        target.Avatar = source.Avatar;
        target.LanguageId = source.LanguageId;
        target.Roles = source.Roles;
        target.Claims = source.Claims;
        target.LockoutEnd = source.LockoutEnd;
      });

      config.CreateMap<UserEditModel, User>((source, target) =>
      {
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.IsSuper = source.IsSuper;
        target.Email = source.Email;
        target.Avatar = source.Avatar;
        target.LanguageId = source.LanguageId;
        target.Roles = source.Roles;
        target.Claims = source.Claims;
        target.LockoutEnd = source.LockoutEnd;
      });
    }
  }
}
