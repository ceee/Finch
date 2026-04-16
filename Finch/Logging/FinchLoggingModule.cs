using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Finch.Logging;

public class FinchLoggingModule : FinchModule
{
  public override int Order { get; } = -100;


  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddLogging(builder =>
    {
      // get seq configuration
      IConfigurationSection seqConfig = configuration.GetSection("Finch:Seq");
      FinchSeqOptions seqOptions = seqConfig.Get<FinchSeqOptions>() ?? new();

      // default level
      builder.SetMinimumLevel(seqOptions.MinimumLevel);

      // apply log level overrides from finch
      foreach (KeyValuePair<string, LogLevel> levelOverride in seqOptions.LevelOverrides)
      {
        builder.AddFilter(levelOverride.Key, levelOverride.Value);
      }

      // add optional seq sink
      if (seqConfig.Exists() && seqOptions.ApiKey.HasValue())
      {
        builder.AddSeq(seqOptions.ServerUrl, seqOptions.ApiKey, seqOptions.Enrichers);
      }
    });
  }
}