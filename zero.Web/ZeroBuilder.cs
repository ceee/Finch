using Microsoft.Extensions.DependencyInjection;

namespace zero.Web
{
  public class ZeroBuilder
  {
    public virtual IServiceCollection Services { get; }


    public ZeroBuilder(IServiceCollection services)
    {
      Services = services;
    }


    //public virtual AuthenticationBuilder AddPolicyScheme(string authenticationScheme, string displayName, Action<PolicySchemeOptions> configureOptions);
   
    //public virtual AuthenticationBuilder AddRemoteScheme<TOptions, THandler>(string authenticationScheme, string displayName, Action<TOptions> configureOptions)
    //  where TOptions : RemoteAuthenticationOptions, new()
    //  where THandler : RemoteAuthenticationHandler<TOptions>;
  }
}
