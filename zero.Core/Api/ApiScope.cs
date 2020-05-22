using System;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class ApiScope
  {
    public string AppId { get; set; }

    public bool IncludeShared { get; set; }

    public bool Global { get; set; }

    public bool IsAppAware => !Global && !AppId.IsNullOrEmpty();

    public bool IsAllowed(string appId)
    {
      if (!IsAppAware)
      {
        return true;
      }

      if (IncludeShared && appId.Equals(Constants.Database.SharedAppId, StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }

      return appId.Equals(AppId, StringComparison.OrdinalIgnoreCase);
    }
  }
}
