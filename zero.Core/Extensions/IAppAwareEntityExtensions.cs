using System;
using System.Globalization;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Extensions
{
  public static class IAppAwareEntityExtensions
  {
    public static bool InScope(this IAppAwareEntity model, string currentAppId)
    {
      bool includeShared = typeof(IAppAwareShareableEntity).IsAssignableFrom(model.GetType());

      if (currentAppId.IsNullOrWhiteSpace() || model.AppId.IsNullOrWhiteSpace())
      {
        return false;
      }

      if (includeShared && model.AppId.Equals(Constants.Database.SharedAppId, StringComparison.InvariantCultureIgnoreCase))
      {
        return true;
      }

      return model.AppId.Equals(currentAppId, StringComparison.OrdinalIgnoreCase);
    }
  }
}
