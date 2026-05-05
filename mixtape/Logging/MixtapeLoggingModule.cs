using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mixtape.Logging;

public class MixtapeLoggingModule : MixtapeModule
{
  public override int Order { get; } = -100;


  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddHttpClient<AnalyticsController>().RemoveAllLoggers();
    services.AddOptions<AnalyticsOptions>().Bind(configuration.GetSection("Mixtape:Analytics"));

    services.AddLogging(builder =>
    {
      // get seq configuration
      IConfigurationSection seqConfig = configuration.GetSection("Mixtape:Seq");
      MixtapeSeqOptions seqOptions = seqConfig.Get<MixtapeSeqOptions>() ?? new();

      // default level
      builder.SetMinimumLevel(seqOptions.MinimumLevel);

      // apply log level overrides from mixtape
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