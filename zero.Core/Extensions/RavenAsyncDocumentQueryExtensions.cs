using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Extensions
{
  public static class RavenAsyncDocumentQueryExtensions
  {
    static Type _appAwareEntity = typeof(IAppAwareEntity);

    public static IAsyncDocumentQuery<T> Scope<T>(this IAsyncDocumentQuery<T> source, string appId, bool includeShared = true)
    {
      if (appId.IsNullOrEmpty() || !_appAwareEntity.IsAssignableFrom(typeof(T)))
      {
        return source;
      }

      HashSet<string> ids = new HashSet<string>();
      ids.Add(appId);

      if (includeShared)
      {
        ids.Add(Constants.Database.SharedAppId);
      }

      return source.WhereIn(nameof(IAppAwareEntity.AppId), ids);
    }


    public static IAsyncDocumentQuery<T> Scope<T>(this IAsyncDocumentQuery<T> source, ApiScope scope)
    {
      if (scope == null || scope.IsShared)
      {
        return source;
      }

      if (scope.AppId.IsNullOrEmpty() || !_appAwareEntity.IsAssignableFrom(typeof(T)))
      {
        return source;
      }

      HashSet<string> ids = new HashSet<string>();
      ids.Add(scope.AppId);

      if (scope.IncludeShared)
      {
        ids.Add(Constants.Database.SharedAppId);
      }

      return source.WhereIn(nameof(IAppAwareEntity.AppId), ids);
    }
  }
}
