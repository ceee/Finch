using System;
using zero.Core.Entities;
using zero.Web.Models;

namespace zero.Web.Mapper
{
  public class UserMapperConfig : IMapperConfig
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

      config.CreateMap<User, UserListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive && (!source.LockoutEnabled || !source.LockoutEnd.HasValue);
        target.Email = source.Email;
        target.Avatar = "https://fifty.brothers.studio/Media/Avatars/tobi.jpg"; // TODO //source.Avatar?.Source;
        target.Roles = String.Join(", ", source.Roles); // TODO get name from alias
      });

      config.CreateMap<UserRole, UserRoleEditModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.CreatedDate = source.CreatedDate;
        target.Description = source.Description;
        target.Icon = source.Icon;
        target.Claims = source.Claims;
      });

      config.CreateMap<UserRoleEditModel, UserRole>((source, target) =>
      {
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.Description = source.Description;
        target.Icon = source.Icon;
        target.Claims = source.Claims;
      });

      config.CreateMap<UserRole, UserRoleListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.CountClaims = source.Claims.Count;
        target.Icon = source.Icon;
      });
    }
  }
}
