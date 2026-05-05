using Microsoft.Extensions.Logging;
using Seq.Extensions.Logging;

namespace Mixtape.Logging;

public class MixtapeSeqOptions
{
  /// <summary>
  /// URL to the Seq server
  /// </summary>
  public string ServerUrl { get; set; } = "http://localhost:5341";

  /// <summary>
  /// A Seq API key to authenticate or tag messages from the logger
  /// </summary>
  public string ApiKey { get; set; }

  /// <summary>
  /// The level below which events will be suppressed
  /// </summary>
  public LogLevel MinimumLevel { get; set; } = LogLevel.Information;

  /// <summary>
  /// A dictionary mapping logger name prefixes to minimum logging levels.
  /// </summary>
  public Dictionary<string, LogLevel> LevelOverrides { get; set; } = new LogLevelOverrides();

  /// <summary>
  /// A collection of enrichers to apply.
  /// </summary>
  public List<Action<EnrichingEvent>> Enrichers { get; set; } = [];
}