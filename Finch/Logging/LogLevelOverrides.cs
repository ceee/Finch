using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Finch.Logging;

public class LogLevelOverrides : Dictionary<string, LogLevel>
{
  public LogLevelOverrides()//IHostEnvironment env)
  {
    this["finch"] = LogLevel.Debug;
    this["Finch.Routing"] = LogLevel.Debug;

    // if (env.IsDevelopment())
    // {
    //   this["finch"] = LogLevel.Debug;
    //   this["Finch.Routing"] = LogLevel.Debug;
    // }
    // else
    // {
    //   this["finch"] = LogLevel.Debug;
    // }

    this["SixLabors"] = LogLevel.Warning;
    this["Quartz"] = LogLevel.Warning;
    this["Microsoft.AspNetCore"] = LogLevel.Warning;

    string executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    if (executingAssemblyName != null)
    {
      this[executingAssemblyName] = LogLevel.Debug;
    }
  }
}