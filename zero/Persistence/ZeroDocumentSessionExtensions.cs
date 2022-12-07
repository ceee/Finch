namespace zero.Persistence;

public static class ZeroDocumentSessionExtensions
{ 
  public static void SetCollection<T>(this IZeroDocumentSession session, T model, string collectionName)
  {
    session.Advanced.GetMetadataFor(model)[Raven.Client.Constants.Documents.Metadata.Collection] = collectionName;
  }

  public static void Expires<T>(this IZeroDocumentSession session, T model, TimeSpan expires)
  {
    session.Advanced.GetMetadataFor(model)[Raven.Client.Constants.Documents.Metadata.Expires] = DateTime.UtcNow.AddSeconds(expires.TotalSeconds);
  }
}