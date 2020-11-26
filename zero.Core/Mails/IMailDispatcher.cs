using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace zero.Core.Mails
{
  public interface IMailDispatcher : IDisposable
  {
    /// <summary>
    /// Adds a new mail message to the outgoing queue
    /// </summary>
    void Enqueue(Mail message);

    /// <summary>
    /// Sends all mails which have been added to the queue previously
    /// </summary>
    Task Send(CancellationToken token = default);
  }
}
