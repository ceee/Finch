using zero.Applications;

namespace zero.Demo;

public class DevApplicationResolverHandler : IApplicationResolverHandler
{
  IWebHostEnvironment Env;

  static Dictionary<int, string> PortMap = new()
  {
    { 2310, "app.hofbauer" },
    { 2100, "app.hofbauer" },
    { 2300, "app.brothers" },
    { 2320, "app.sporthuber" }
  };

  public DevApplicationResolverHandler(IWebHostEnvironment env)
  {
    Env = env;
  }


  public Application Resolve(HttpRequest request, IEnumerable<Application> applications)
  {
    //if (!Env.IsDevelopment())
    //{
    //  return null;
    //}
    if (request.Host.Host.Contains("ngrok.io"))
    {
      return applications.First(x => x.Id == "app.hofbauer");
    }

    if (request.Host.Port.HasValue && PortMap.TryGetValue(request.Host.Port.Value, out string appId))
    {
      return applications.FirstOrDefault(x => x.Id == appId);
    }

    return null;
  }
}