using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace zero.Logging;

public class ZeroLoggingModule : ZeroModule
{
  public override int Order { get; } = -100;


  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddLogging(builder =>
    {
      // get seq configuration
      IConfigurationSection seqConfig = configuration.GetSection("Zero:Seq");
      ZeroSeqOptions seqOptions = seqConfig.Get<ZeroSeqOptions>();

      Console.WriteLine(seqOptions.ApiKey);
      Console.WriteLine(seqOptions.ServerUrl);
      Console.WriteLine("use seq: " + seqConfig.Exists());

      // default level
      builder.SetMinimumLevel(seqOptions.MinimumLevel);

      // apply log level overrides from zero
      foreach (KeyValuePair<string, LogLevel> levelOverride in seqOptions.LevelOverrides)
      {
        builder.AddFilter(levelOverride.Key, levelOverride.Value);
      }

      // add optional seq sink
      if (seqConfig.Exists())
      {
        builder.AddSeq(seqOptions.ServerUrl, seqOptions.ApiKey, seqOptions.Enrichers);
      }
    });
  }
}