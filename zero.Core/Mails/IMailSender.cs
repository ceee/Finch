using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Mails
{
  public interface IMailSender
  {
    /// <summary>
    /// Send a mail
    /// </summary>
    //Task<Mail> Create(IMailTemplate template);

    /// <summary>
    /// Send a mail
    /// </summary>
    //Task<MailSendResult> Send(Mail message, CancellationToken token = default);
  }
}
