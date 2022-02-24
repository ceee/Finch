using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace zero.Mails;

public class MailProvider : IMailProvider
{
  protected IMailTemplatesStore Collection { get; set; }

  protected ILogger<IMailProvider> Logger { get; set; }

  protected IZeroContext Zero { get; set; }

  protected IMailDispatcher MailSender { get; set; }

  protected IRazorRenderer Renderer { get; set; }

  protected Regex PlaceholderRegex { get; set; }

  private Encoding encoding = Encoding.UTF8;


  public MailProvider(IZeroContext zero, IMailTemplatesStore collection, ILogger<IMailProvider> logger, IMailDispatcher mailSender, IRazorRenderer renderer)
  {
    Zero = zero;
    Collection = collection;
    Logger = logger;
    MailSender = mailSender;
    Renderer = renderer;
    PlaceholderRegex = new Regex("{([\\w-_.]+)}", RegexOptions.IgnoreCase);
  }


  /// <inheritdoc />
  public virtual async Task<T> Create<T>(string mailTemplateKey, Action<MailTemplate> onCreate = null) where T : Mail, new()
  {
    MailTemplate template = await GetMailTemplate(mailTemplateKey);

    if (template == null)
    {
      Logger.LogError("Could not find a mail template with the key {key}", mailTemplateKey);
      return null;
    }

    onCreate?.Invoke(template);

    return Merge(new T(), template);
  }


  /// <inheritdoc />
  public virtual async Task<Mail> Create(string mailTemplateKey, Action<MailTemplate> onCreate = null)
  {
    MailTemplate template = await GetMailTemplate(mailTemplateKey);

    if (template == null)
    {
      Logger.LogError("Could not find a mail template with the key {key}", mailTemplateKey);
      return null;
    }

    onCreate?.Invoke(template);

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
    if (message.IsDeactivated)
    {
      return;
    }

    try
    {
      await Render(message);
      dispatcher.Enqueue(message);
      await dispatcher.Send(token);
      Logger.LogInformation("Dispatched email (template: {template}) to {recipient} (cc: {cc}, bcc: {bcc})", message.Template?.Alias ?? "none", message.To, message.CC, message.Bcc);
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, "Failed to send mail message with template @{template}", message.Template.Key);
    }
  }


    
  protected virtual async Task<MailTemplate> GetMailTemplate(string key)
  {
    MailTemplate mailTemplate = await Collection.GetByKey(key);

    if (mailTemplate != null)
    {
      mailTemplate = ObjectCopycat.Clone(mailTemplate);
    }

    return mailTemplate;
  }
     

  /// <inheritdoc />
  public virtual async Task<string> Render(Mail message)
  {
    message.Subject = TokenReplacement.Apply(message.Subject, message.Placeholders);
    message.Body = TokenReplacement.Apply(message.Body, message.Placeholders);
    message.Preheader = TokenReplacement.Apply(message.Preheader, message.Placeholders);

    if (!message.HasView)
    {
      message.IsRendered = true;
      return message.Body;
    }

    string viewPath = message.ViewPath;

    if (viewPath.IsNullOrEmpty())
    {
      viewPath = Zero.Options.For<MailOptions>().BuildViewPath(message);
    }

    message.Body = await Renderer.ViewAsync(viewPath, message);
    message.IsBodyHtml = true;
    message.IsRendered = true;
    return message.Body;
  }


  protected virtual T Merge<T>(T mail, MailTemplate template) where T : Mail
  {
    mail.Template = template;
    mail.IsDeactivated = !mail.Template.IsActive;

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
    mail.Preheader = mail.Template.Preheader;

    return mail;
  }
}


public interface IMailProvider
{
  /// <summary>
  /// Creates a maling from a template
  /// </summary>
  Task<Mail> Create(string mailTemplateKey, Action<MailTemplate> onCreate = null);

  /// <summary>
  /// Creates a maling from a template
  /// </summary>
  Task<T> Create<T>(string mailTemplateKey, Action<MailTemplate> onCreate = null) where T : Mail, new();

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
