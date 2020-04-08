using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core
{
  public class ZeroAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, IZeroEntity>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,OperationAuthorizationRequirement requirement, IZeroEntity resource)
    {
      //context.User.Claims.FirstOrDefault(x => x.Type == "yo")
      if (requirement.Name == Operations.Read.Name) // context.User.Identity?.Name == resource.Author &&
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }


    public static class Operations
    {
      public static OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = nameof(Create) };
      public static OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = nameof(Read) };
      public static OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = nameof(Update) };
      public static OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = nameof(Delete) };
    }
  }
}
