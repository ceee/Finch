using Microsoft.Extensions.Options;
using System.Collections.Generic;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class PermissionsApi : IPermissionsApi
  {
    protected ZeroOptions Options { get; set; }


    public PermissionsApi (IOptionsMonitor<ZeroOptions> options)
    {
      Options = options.CurrentValue;
    }


    /// <inheritdoc />
    //public IList<PermissionCollection> GetPermissions()
    //{

    //}
  }


  public interface IPermissionsApi
  {
    
  }
}
