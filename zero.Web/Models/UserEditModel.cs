using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class UserEditModel : EditModel
  {
    public string Name { get; set; }

    public bool IsSuper { get; set; }

    public string Email { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public Media Avatar { get; set; }

    public string LanguageId { get; set; }

    public List<string> Roles { get; set; } = new List<string>();

    public List<IUserClaim> Claims { get; set; } = new List<IUserClaim>();

    public DateTimeOffset? LockoutEnd { get; set; }
  }
}
