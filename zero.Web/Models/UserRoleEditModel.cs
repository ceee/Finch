using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class UserRoleEditModel : EditModel
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    // TODO use IUserClaim and resolve to default type
    // see here: http://www.dotnet-programming.com/post/2017/05/08/Aspnet-core-Deserializing-Json-with-Dependency-Injection.aspx
    public List<UserClaim> Claims { get; set; }
  }
}
