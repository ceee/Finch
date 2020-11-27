using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Renderer;

namespace zero.Core.Mails
{
  public class MailProvider : IMailProvider
  {
    protected IMailTemplatesCollection Collection { get; set; }

    protected ILogger<IMailProvider> Logger { get; set; }

    protected IZeroContext Zero { get; set; }

    protected IMailDispatcher MailSender { get; set; }

    protected IRazorRenderer Renderer { get; set; }

    protected Regex PlaceholderRegex { get; set; }

    private Encoding encoding = Encoding.UTF8;


    public MailProvider(IZeroContext zero, IMailTemplatesCollection collection, ILogger<IMailProvider> logger, IMailDispatcher mailSender, IRazorRenderer renderer)
    {
      Zero = zero;
      Collection = collection;
      Logger = logger;
      MailSender = mailSender;
      Renderer = renderer;
      PlaceholderRegex = new Regex("{([\\w-_.]+)}", RegexOptions.IgnoreCase);
    }


    /// <inheritdoc />
    public virtual async Task<Mail<T>> Create<T>(string mailTemplateKey, T model, CancellationToken token = default) where T : class
    {
      IMailTemplate template = await GetMailTemplate(mailTemplateKey);

      if (template == null)
      {
        Logger.LogError("Could not find a mail template with the key {key}", mailTemplateKey);
        return null;
      }

      Mail<T> mail = Merge(new Mail<T>(), template);
      mail.Model = model;
      return mail;
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

      return Merge(new Mail(), template);
    }


    /// <inheritdoc />
    public virtual async Task Send(Mail message, CancellationToken token = default)
    {
      await Send(message, MailSender, token);
    }


    /// <inheritdoc />
    public virtual async Task Send(Mail message, IMailDispatcher dispatcher, CancellationToken token = default)
    {
      await Render(message);
      dispatcher.Enqueue(message);
      await dispatcher.Send(token);
    }


    
    protected virtual async Task<IMailTemplate> GetMailTemplate(string key)
    {
      return await Collection.GetByKey(key);
    }


    /// <inheritdoc />
    public virtual async Task<string> Render(Mail message)
    {
      message.Subject = TokenReplacement.Apply(message.Subject, message.Placeholders);
      message.Body = TokenReplacement.Apply(message.Body, message.Placeholders);

      if (!message.HasView)
      {
        message.IsRendered = true;
        return message.Body;
      }

      string viewPath = message.ViewPath;

      if (viewPath.IsNullOrEmpty())
      {
        viewPath = $"~/Views/Mails/{message.Template.Key.Replace('.', '/')}.cshtml";
      }

      message.Body = await Renderer.ViewAsync(viewPath, message);
      message.IsBodyHtml = true;
      message.IsRendered = true;
      return message.Body;
    }


    protected virtual T Merge<T>(T mail, IMailTemplate template) where T : Mail
    {
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
  }


  public interface IMailProvider
  {
    /// <inheritdoc />
    Task<Mail> Create(string mailTemplateKey, CancellationToken token = default);

    /// <inheritdoc />
    Task<Mail<T>> Create<T>(string mailTemplateKey, T model, CancellationToken token = default) where T : class;

    /// <summary>
    /// Renders the message body.
    /// This is automatically called when sending messages.
    /// </summary>
    Task<string> Render(Mail message);

    /// <summary>
    /// Sends a message with the default dispatcher
    /// </summary>
    Task Send(Mail message, CancellationToken token = default);

    /// <summary>
    /// Sends a message with the specified dispatcher
    /// </summary>
    Task Send(Mail message, IMailDispatcher dispatcher, CancellationToken token = default);
  }
}
