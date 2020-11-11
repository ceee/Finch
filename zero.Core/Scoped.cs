using zero.Core.Api;

namespace zero.Core
{
  public class Scoped<T> where T : IAppAwareBackofficeApi
  {
    T Api;
    IApplicationContext AppContext;

    public Scoped(T api, IApplicationContext appContext)
    {
      Api = api;
      AppContext = appContext;
    }


    /// <summary>
    /// Get Api with automatically resolved application
    /// </summary>
    public T Current
    {
      get
      {
        Api.Scope.AppId = AppContext.AppId;
        Api.Scope.IsShared = false;
        return Api;
      }
    }


    /// <summary>
    /// Get Api which has its scoped set to all applications (including shared)
    /// </summary>
    public T All
    {
      get
      {
        Api.Scope.AppId = null;
        Api.Scope.IsShared = true;
        return Api;
      }
    }


    /// <summary>
    /// Get Api with scope set to only shared entities
    /// </summary>
    public T Shared
    {
      get
      {
        Api.Scope.AppId = Constants.Database.SharedAppId;
        Api.Scope.IsShared = false;
        return Api;
      }
    }


    /// <summary>
    /// Get Api with scope set to the requested app id
    /// </summary>
    public T App(string id)
    {
      Api.Scope.AppId = id;
      Api.Scope.IsShared = false;
      return Api;
    }
  }
}
