using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Mixtape.Logging;

public class LogLevelOverrides : Dictionary<string, LogLevel>
{
  public LogLevelOverrides()//IHostEnvironment env)
  {
    this["Mixtape"] = LogLevel.Debug;
    this["Mixtape.Routing"] = LogLevel.Debug;

    // if (env.IsDevelopment())
    // {
    //   this["Mixtape"] = LogLevel.Debug;
    //   this["Mixtape.Routing"] = LogLevel.Debug;
    // }
    // else
    // {
    //   this["Mixtape"] = LogLevel.Debug;
    // }

    this["SixLabors"] = LogLevel.Warning;
    this["Quartz"] = LogLevel.Warning;
    this["Microsoft.AspNetCore"] = LogLevel.Warning;
    this["System.Net.Http.HttpClient"] = LogLevel.Warning;
    this["Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager"] = LogLevel.Error;

    string entryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
    if (entryAssemblyName != null)
    {
      this[entryAssemblyName] = LogLevel.Debug;
    }
  }
}