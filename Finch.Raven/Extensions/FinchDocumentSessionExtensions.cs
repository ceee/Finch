using Rv = Raven.Client;

namespace Finch.Raven;

public static class FinchDocumentSessionExtensions
{ 
  public static void SetCollection<T>(this IAsyncDocumentSession session, T model, string collectionName)
  {
    session.Advanced.GetMetadataFor(model)[Rv.Constants.Documents.Metadata.Collection] = collectionName;
  }

  public static void Expires<T>(this IAsyncDocumentSession session, T model, TimeSpan expires)
  {
    session.Advanced.GetMetadataFor(model)[Rv.Constants.Documents.Metadata.Expires] = DateTime.UtcNow.AddSeconds(expires.TotalSeconds);
  }
}