using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Finch.Mails;

public class MailProvider : IMailProvider
{
  protected ILogger<IMailProvider> Logger { get; set; }

  protected IFinchContext Finch { get; set; }

  protected IMailDispatcher MailSender { get; set; }

  protected IRazorRenderer Renderer { get; set; }

  protected MailOptions Options { get; set; }

  private Encoding encoding = Encoding.UTF8;


  public MailProvider(IFinchContext finch, ILogger<IMailProvider> logger, IMailDispatcher mailSender, IRazorRenderer renderer)
  {
    Finch = Finch;
    Logger = logger;
    MailSender = mailSender;
    Renderer = renderer;
    Options = Finch.Options.For<MailOptions>();
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
      await Prepare(message);
      dispatcher.Enqueue(message);
      await dispatcher.Send(token);

      Logger.LogInformation("Dispatched email to {recipient} (cc: {cc}, bcc: {bcc})", message.To, message.CC, message.Bcc);
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, "Failed to send mail message");
    }
  }
     

  /// <inheritdoc />
  public virtual async Task<string> Prepare(Mail message)
  {
    message.From ??= new MailAddress(Options.SenderEmail, Options.SenderName, encoding);
    message.Sender ??= message.From;

    if (message.ReplyToList.Count < 1)
    {
      message.ReplyToList.Add(message.From);
    }

    message.Subject = TokenReplacement.Apply(message.Subject, message.Placeholders);
    message.SubjectEncoding = encoding;
    message.Body = TokenReplacement.Apply(message.Body, message.Placeholders);
    message.BodyEncoding = encoding;
    message.Preheader = TokenReplacement.Apply(message.Preheader, message.Placeholders);

    string appName = Finch.Options.AppName.Or(Assembly.GetEntryAssembly()?.GetName().Name);
    message.Metadata.Add("application", appName);

    if (!message.HasView || message.Body.HasValue())
    {
      message.IsRendered = true;
      return message.Body;
    }

    string viewPath = message.ViewPath;

    if (viewPath.IsNullOrEmpty())
    {
      viewPath = Options.BuildViewPath(message);
    }

    message.Body = await Renderer.ViewAsync(viewPath, message);
    message.IsBodyHtml = true;
    message.IsRendered = true;
    return message.Body;
  }
}


public interface IMailProvider
{
  /// <summary>
  /// Renders the message body.
  /// This is automatically called when sending messages.
  /// </summary>
  Task<string> Prepare(Mail message);

  /// <summary>
  /// Sends a message with the default dispatcher
  /// </summary>
  Task Send(Mail message, CancellationToken token = default);

  /// <summary>
  /// Sends a message with the specified dispatcher
  /// </summary>
  Task Send(Mail message, IMailDispatcher dispatcher, CancellationToken token = default);
}
