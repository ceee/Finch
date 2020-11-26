using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Mails
{
  public class MailProvider : IMailProvider
  {
    protected IMailTemplatesCollection Collection { get; set; }

    protected ILogger<IMailProvider> Logger { get; set; }

    protected IZeroContext Zero { get; set; }

    protected IMailSender MailSender { get; set; }

    protected Regex PlaceholderRegex { get; set; }

    private Encoding encoding = Encoding.UTF8;


    public MailProvider(IZeroContext zero, IMailTemplatesCollection collection, ILogger<IMailProvider> logger, IMailSender mailSender)
    {
      Zero = zero;
      Collection = collection;
      Logger = logger;
      MailSender = mailSender;
      PlaceholderRegex = new Regex("{([\\w-_.]+)}", RegexOptions.IgnoreCase);
    }


    /// <inheritdoc />
    public virtual async Task<Mail> Create(string mailTemplateKey, CancellationToken token = default)
    {
      IMailTemplate template = await GetMailTemplate(mailTemplateKey);

      if (template == null)
      {
        Logger.LogError("Could not find a mail template with the key {key}", mailTemplateKey);
        return null;
      }

      Mail mail = new Mail();

      mail.Template = template;

      // get sender from template or fall back to application
      mail.From = new MailAddress(template.SenderEmail.Or(Zero.Application.Email), template.SenderName.Or(Zero.Application.FullName), encoding);
      mail.Sender = mail.From;
      mail.ReplyToList.Add(mail.From);

      // cc + bcc (multiple addresses are separated with commas 
      if (!template.Cc.IsNullOrWhiteSpace())
      {
        mail.CC.Add(template.Cc);
      }
      if (!template.Bcc.IsNullOrWhiteSpace())
      {
        mail.Bcc.Add(template.Bcc);
      }

      // recipient
      if (!template.RecipientEmail.IsNullOrWhiteSpace())
      {
        mail.To.Add(template.RecipientEmail);
      }

      // subject
      mail.Subject = template.Subject;
      mail.SubjectEncoding = encoding;

      // body
      mail.Body = mail.Template.Body;
      mail.IsBodyHtml = true;
      mail.BodyEncoding = encoding;

      return mail;
    }


    /// <inheritdoc />
    public virtual async Task Send(Mail message, CancellationToken token = default)
    {
      await Send(message, MailSender, token);
    }


    /// <inheritdoc />
    public virtual async Task Send(Mail message, IMailSender sender, CancellationToken token = default)
    {
      await sender.Send(message, token);
    }


    /// <inheritdoc />
    protected virtual async Task<IMailTemplate> GetMailTemplate(string key)
    {
      return await Collection.GetByKey(key);
    }


    //protected Mail ReplacePlaceholders(Mail message)
    //{
      

    //  foreach ((string key, string value) in message.Placeholders)
    //  {
        
    //  }
    //}


    //protected string ReplacePlaceholders(string text)
    //{
    //  MatchCollection matches = PlaceholderRegex.Matches(text);

    //  foreach (Match match in matches)
    //  {
    //    text.
    //  }
    //}
  }


  public interface IMailProvider
  {
    /// <inheritdoc />
    Task<Mail> Create(string mailTemplateKey, CancellationToken token = default);

    /// <inheritdoc />
    Task Send(Mail message, CancellationToken token = default);

    /// <inheritdoc />
    Task Send(Mail message, IMailSender sender, CancellationToken token = default);
  }
}
