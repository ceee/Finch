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


  public bool TryResolve(HttpContext context, IEnumerable<Application> applications, out Application resolved)
  {
    resolved = null;

    //if (!Env.IsDevelopment())
    //{
    //  return false;
    //}

    if (context.Request.Host.Host.Contains("ngrok.io"))
    {
      resolved = applications.First(x => x.Id == "app.hofbauer");
      return true;
    }

    if (context.Request.Host.Port.HasValue && PortMap.TryGetValue(context.Request.Host.Port.Value, out string appId))
    {
      resolved = applications.FirstOrDefault(x => x.Id == appId);
      return true;
    }

    return false;
  }
}