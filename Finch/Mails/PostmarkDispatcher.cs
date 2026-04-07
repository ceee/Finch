using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace Finch.Mails;

public class PostmarkDispatcher : IMailDispatcher
{
  protected Queue<Mail> Queue { get; } = new();

  protected PostmarkClient Postmark { get; set; }

  protected PostmarkAdminClient PostmarkAdmin { get; set; }

  protected MailOptions Options { get; set; }

  protected IWebHostEnvironment Env { get; set; }


  public PostmarkDispatcher(IOptionsMonitor<MailOptions> monitor, IWebHostEnvironment env)
  {
    Options = monitor.CurrentValue;
    Postmark = new(Options.Postmark.ServerToken);
    PostmarkAdmin = new(Options.Postmark.AccountToken);
    Env = env;

    monitor.OnChange(opts =>
    {
      Options = opts;
      Postmark = new(Options.Postmark.ServerToken);
    });
  }


  /// <inheritdoc />
  public async Task<bool> IsSenderSupported(string email)
  {
    if (email.IsNullOrWhiteSpace() || Options.Postmark.AccountToken.IsNullOrEmpty())
    {
      return true;
    }

    try
    {
      email = email.FullTrim();
      PostmarkSenderSignatureList signatures = await PostmarkAdmin.GetSenderSignaturesAsync();
      return signatures.SenderSignatures.Any(x => x.Confirmed && x.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));
    }
    catch
    {
      return true;
    }
  }


  /// <inheritdoc />
  public void Enqueue(Mail message)
  {
    Queue.Enqueue(message);
  }


  /// <inheritdoc />
  public async Task Send(CancellationToken token = default)
  {
    HashSet<PostmarkMessage> messages = [];

    while (Queue.Count > 0)
    {
      Mail message = Queue.Dequeue();

      PostmarkMessage data = new()
      {
        // to addresses
        To = message.To.ToString(),
        Cc = message.CC.ToString(),
        Bcc = message.Bcc.ToString(),

        // from address
        From = message.From.ToString(),
        ReplyTo = message.ReplyToList.ToString(),

        // subject
        Subject = message.Subject,

        // tracking
        TrackLinks = LinkTrackingOptions.None,
        TrackOpens = false,

        // configuration
        MessageStream = Options.Postmark.MessageStream,
        //Tag = message.Template?.Key,
        Metadata = message.Metadata
      };

      // set attachments
      foreach (System.Net.Mail.Attachment attachment in message.Attachments)
      {
        data.AddAttachment(attachment.ContentStream, attachment.Name, attachment.ContentType.MediaType, attachment.ContentId);
      }

      // set body
      if (!message.IsBodyHtml)
      {
        data.TextBody = message.Body;
      }
      else
      {
        data.HtmlBody = message.Body;
      }

      // overwrite for debug mode
      if (Options.Debug || (Env != null && Env.IsDevelopment()))
      {
        data.From = "noreply@post.swcs.pro";

        string[] allowedTlds = ["swcs.pro", "alias.swcs.pro"];
        string tld = data.To.TrimEnd('>').Split('@').LastOrDefault();

        data.Cc = null; // "cee-maildev@gmx.at,anaheimcore@gmail.com,ceemaildev@yahoo.com";
        data.Bcc = null;

        if (!allowedTlds.Contains(tld, StringComparer.InvariantCultureIgnoreCase))
        {
          data.Subject = $"{data.Subject} (test; für {data.To})";
          data.To = "maildev@alias.swcs.pro";
        }
        else 
        {
          data.Subject = $"{data.Subject} (test)";
        }
      }

      messages.Add(data);
    }

    // finally sends the message
    IEnumerable<PostmarkResponse> responses = await Postmark.SendMessagesAsync(messages);

    foreach (PostmarkResponse response in responses)
    {
      if (response.ErrorCode > 0)
      {
        throw new PostmarkSendException($"Could not send message via Postmark API. Code: {response.ErrorCode}, Message: {response.Message}");
      }
    }
  }


  /// <inheritdoc />
  public void Dispose() { }
}


public class PostmarkSendException : Exception
{
  public PostmarkSendException() : base() { }

  public PostmarkSendException(string message) : base(message) { }
}
