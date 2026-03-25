using System.Reflection;
using Microsoft.Extensions.Logging;

namespace zero.Logging;

public class LogLevelOverrides : Dictionary<string, LogLevel>
{
  public LogLevelOverrides()//IHostEnvironment env)
  {
    this["zero"] = LogLevel.Debug;
    this["zero.Routing"] = LogLevel.Debug;

    // if (env.IsDevelopment())
    // {
    //   this["zero"] = LogLevel.Debug;
    //   this["zero.Routing"] = LogLevel.Debug;
    // }
    // else
    // {
    //   this["zero"] = LogLevel.Debug;
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