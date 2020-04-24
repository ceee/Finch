using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class UserRoleEditModel : EditModel
  {
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public List<IUserClaim> Claims { get; set; }
  }
}
