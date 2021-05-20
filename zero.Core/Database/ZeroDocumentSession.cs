using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;

namespace zero.Core.Database
{
  public class ZeroDocumentSession : AsyncDocumentSession, IZeroDocumentSession, IZeroCoreDocumentSession
  {
    public ZeroDocumentSession(DocumentStore documentStore, Guid id, SessionOptions options) : base(documentStore, id, options)
    {

    }
  }

  public interface IZeroDocumentSession : IAsyncDocumentSession
  {

  }


  public interface IZeroCoreDocumentSession : IAsyncDocumentSession
  {

  }
}
