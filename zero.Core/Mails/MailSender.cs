using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Mails
{
  public class MailSender : IMailSender
  {
    protected IMailTemplatesCollection Collection { get; set; }

    protected SmtpClient Client { get; set; }

    protected ILogger<IMailSender> Logger { get; set; }

    protected IZeroContext Zero { get; set; }

    private Encoding encoding = Encoding.UTF8;


    public MailSender(IZeroContext zero, IMailTemplatesCollection collection, ILogger<IMailSender> logger)
    {
      Zero = zero;
      Collection = collection;
      Logger = logger;
      Client = new SmtpClient();
    }


    /// <inheritdoc />
    //public virtual async Task<MailMessage> Create(string mailTemplateKey, CancellationToken token = default)
    //{
    //  IMailTemplate template = await GetMailTemplate(mailTemplateKey);

    //  if (template == null)
    //  {
    //    Logger.LogError("Could not find a mail template with the key {key}", mailTemplateKey);
    //    return null;
    //  }

    //  MailMessage message = new MailMessage();

    //  // get sender from template or fall back to application
    //  message.From = message.Sender;
    //  message.Sender = new MailAddress(template.SenderEmail.Or(Zero.App.Email), template.SenderName.Or(Zero.App.FullName), encoding);
    //  message.ReplyToList.Add(message.From);

    //  // cc + bcc (multiple addresses are separated with commas 
    //  if (!template.Cc.IsNullOrWhiteSpace())
    //  {
    //    message.CC.Add(template.Cc);
    //  }
    //  if (!template.Bcc.IsNullOrWhiteSpace())
    //  {
    //    message.Bcc.Add(template.Bcc);
    //  }

    //  // recipient
    //  if (!template.RecipientEmail.IsNullOrWhiteSpace())
    //  {
    //    message.To.Add(template.RecipientEmail);
    //  }

    //  // subject
    //  message.Subject = template.Subject;
    //  message.SubjectEncoding = encoding;
    //}


    /// <inheritdoc />
    public virtual async Task<bool> Send(string mailTemplateKey, Mail message, CancellationToken token = default)
    {
      message.Template = await GetMailTemplate(mailTemplateKey);
      
      if (message.Template == null)
      {
        Logger.LogError("Could not find a mail template with the key {key}", mailTemplateKey);
        return false;
      }

      return await Send(message, token);
    }


    /// <inheritdoc />
    public virtual async Task<bool> Send(Mail message, CancellationToken token = default)
    {
      await Task.Delay(0);
      return true;
    }


    /// <inheritdoc />
    public virtual async Task<bool> Send(MailMessage message, CancellationToken token = default)
    {
      try
      {
        await Client.SendMailAsync(message, token);
        return true;
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, "Could not send mail message via SmtpClient");
        return false;
      }
    }


    /// <inheritdoc />
    protected virtual async Task<IMailTemplate> GetMailTemplate(string key)
    {
      return await Collection.GetByKey(key);
    }
  }
}
