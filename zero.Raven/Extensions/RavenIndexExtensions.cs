using Raven.Client.Documents.Indexes;

namespace zero.Raven;

public static class RavenIndexExtensions
{
  public static void Throttle(this AbstractCommonApiForIndexes index, TimeSpan delay)
  {
    index.Configuration[RavenConstants.Indexing.ThrottlingTimeIntervalInMs] = delay.TotalMilliseconds.ToString();
  }
}
