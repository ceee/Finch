namespace zero.Core.Api
{
  public class AppScope<T> : IAppScope<T> where T : IAppAwareBackofficeApi
  {
    T Api;
    IApplicationContext AppContext;

    public AppScope(T api, IApplicationContext appContext)
    {
      Api = api;
      AppContext = appContext;
    }


    /// <inheritdoc />
    public T Current
    {
      get
      {
        Api.Scope.AppId = AppContext.AppId;
        Api.Scope.Global = false;
        return Api;
      }
    }


    /// <inheritdoc />
    public T Global
    {
      get
      {
        Api.Scope.AppId = null;
        Api.Scope.Global = true;
        return Api;
      }
    }


    /// <inheritdoc />
    public T Shared
    {
      get
      {
        Api.Scope.AppId = Constants.Database.SharedAppId;
        Api.Scope.Global = false;
        return Api;
      }
    }


    /// <inheritdoc />
    public T App(string id)
    {
      Api.Scope.AppId = id;
      Api.Scope.Global = false;
      return Api;
    }
  }


  public interface IAppScope<T> where T : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Get Api with automatically resolved application
    /// </summary>
    T Current { get; }

    /// <summary>
    /// Get Api which has its scoped set to all applications (including shared)
    /// </summary>
    T Global { get; }

    /// <summary>
    /// Get Api with scope set to only shared entities
    /// </summary>
    T Shared { get; }

    /// <summary>
    /// Get Api with scope set to the requested app id
    /// </summary>
    T App(string id);
  }
}
