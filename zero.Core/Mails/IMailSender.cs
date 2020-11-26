using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace zero.Core.Mails
{
  public interface IMailSender
  {
    /// <inheritdoc />
    Task Send(Mail message, CancellationToken token = default);
  }
}
