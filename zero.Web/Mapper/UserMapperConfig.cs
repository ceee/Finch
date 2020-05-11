using System;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Extensions;
using zero.Web.Models;
using Raven.Client.Documents;
using zero.Core.Mapper;

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
        target.IsLockedOut = source.LockoutEnabled && source.LockoutEnd.HasValue;
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
      });

      config.CreateMap<User, UserListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.IsActive = source.IsActive && (!source.LockoutEnabled || !source.LockoutEnd.HasValue);
        target.Email = source.Email;
        target.Avatar = source.Avatar?.Source;
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
        target.Claims = source.Claims.Cast<UserClaim>().ToList();
      });

      config.CreateMap<UserRoleEditModel, UserRole>((source, target) =>
      {
        target.Name = source.Name;
        target.IsActive = source.IsActive;
        target.Description = source.Description;
        target.Icon = source.Icon;
        target.Claims = source.Claims.Cast<IUserClaim>().ToList();
      });

      config.CreateMap<UserRole, UserRoleListModel>((source, target) =>
      {
        target.Id = source.Id;
        target.Name = source.Name;
        target.Alias = source.Alias;
        target.CountClaims = source.Claims.Count(x =>
        {
          string[] parts = x.Value.Split(':');

          if (parts.Length < 2)
          {
            return true;
          }
          return parts[1] == PermissionsValue.True || parts[1] == PermissionsValue.Read || parts[1] == PermissionsValue.Write;
        });
        target.Icon = source.Icon;
      });
    }
  }
}
