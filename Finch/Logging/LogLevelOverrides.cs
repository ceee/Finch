using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Finch.Logging;

public class LogLevelOverrides : Dictionary<string, LogLevel>
{
  public LogLevelOverrides()//IHostEnvironment env)
  {
    this["Finch"] = LogLevel.Debug;
    this["Finch.Routing"] = LogLevel.Debug;

    // if (env.IsDevelopment())
    // {
    //   this["Finch"] = LogLevel.Debug;
    //   this["Finch.Routing"] = LogLevel.Debug;
    // }
    // else
    // {
    //   this["Finch"] = LogLevel.Debug;
    // }

    this["SixLabors"] = LogLevel.Warning;
    this["Quartz"] = LogLevel.Warning;
    this["Microsoft.AspNetCore"] = LogLevel.Warning;
    this["System.Net.Http.HttpClient"] = LogLevel.Warning;
    this["Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager"] = LogLevel.Error;

    string executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    if (executingAssemblyName != null)
    {
      this[executingAssemblyName] = LogLevel.Debug;
    }
  }
}