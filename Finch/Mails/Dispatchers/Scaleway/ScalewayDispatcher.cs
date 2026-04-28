using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Attachment = System.Net.Mail.Attachment;

namespace Finch.Mails.Dispatchers.Scaleway;

public class ScalewayDispatcher : IMailDispatcher
{
  /// <inheritdoc />
  public int Priority { get; } = 3;

  protected Queue<Mail> Queue { get; } = new();

  protected MailOptions Options { get; set; }

  protected IWebHostEnvironment Env { get; set; }

  protected HttpClient Http { get; }

  protected JsonSerializerOptions JsonSerializerOptions { get; }

  protected ILogger<ScalewayDispatcher> Logger { get; }


  public ScalewayDispatcher(IOptionsMonitor<MailOptions> monitor, IWebHostEnvironment env, HttpClient http, ILogger<ScalewayDispatcher> logger)
  {
    Options = monitor.CurrentValue;
    Env = env;
    Http = http;
    Http.DefaultRequestHeaders.Add("X-Auth-Token", Options.Scaleway.SecretKey);
    JsonSerializerOptions = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
    Logger = logger;

    monitor.OnChange(opts => Options = opts);
  }


  /// <inheritdoc />
  public bool CanSend()
  {
    return Options.Scaleway != null && !Options.Scaleway.ProjectId.IsNullOrEmpty() && !Options.Scaleway.SecretKey.IsNullOrEmpty();
  }


  /// <inheritdoc />
  public async Task<bool> IsSenderSupported(string email, CancellationToken token = default)
  {
    if (email.IsNullOrWhiteSpace())
    {
      return true;
    }

    try
    {
      string domain = email.FullTrim().Split('@').LastOrDefault();

      string uri = Options.Scaleway.ApiUrl + $"/transactional-email/v1alpha1/regions/{Options.Scaleway.Region}/domains?project_id={Options.Scaleway.ProjectId}&status=checked";
      ScalewayResponse.ListDomains response = await Http.GetFromJsonAsync<ScalewayResponse.ListDomains>(uri, JsonSerializerOptions, token);
      return response.Domains.Any(x => x.Name.Equals(domain, StringComparison.InvariantCultureIgnoreCase));
    }
    catch
    {
      return true;
    }
  }


  /// <inheritdoc />
  public async Task Send(Mail message, CancellationToken token = default)
  {
    string uri = Options.Scaleway.ApiUrl + $"/transactional-email/v1alpha1/regions/{Options.Scaleway.Region}/emails";

    ScalewayRequest.SendEmail data = new()
    {
      // to addresses
      To = Convert(message.To),
      Cc = Convert(message.CC),
      Bcc = Convert(message.Bcc),

      // from address
      From = new ScalewayRequest.EmailAddress()
      {
        Email = message.From!.Address,
        Name = message.From.DisplayName
      },

      // subject
      Subject = message.Subject
    };

    // add reply-to header
    if (message.ReplyToList.Any())
    {
      data.AdditionalHeaders.Add(new()
      {
        Key = "Reply-To",
        Value = message.ReplyToList.ToString()
      });
    }

    // add additional headers
    if (message.Metadata.Any())
    {
      foreach (KeyValuePair<string, string> item in message.Metadata)
      {
        data.AdditionalHeaders.Add(new()
        {
          Key = item.Key,
          Value = item.Value
        });
      }
    }

    // set attachments
    foreach (Attachment attachment in message.Attachments)
    {
      data.Attachments.Add(Convert(attachment));
    }

    // set body
    if (!message.IsBodyHtml)
    {
      data.Text = message.Body;
    }
    else
    {
      data.Html = message.Body;
    }

    // overwrite for debug mode
    if (Options.Debug || (Env != null && !Env.IsProduction()))
    {
      data.From = new ScalewayRequest.EmailAddress()
      {
        Email = "noreply@post.swcs.pro"
      };
      data.Cc = null; // "cee-maildev@gmx.at,anaheimcore@gmail.com,ceemaildev@yahoo.com";
      data.Bcc = null;
      data.To = [new ScalewayRequest.EmailAddress()
      {
        Email = "maildev@alias.swcs.pro"
      }];
      data.Subject = $"{data.Subject} (test)";
    }

    try
    {
      using HttpResponseMessage responseMessage = await Http.PostAsJsonAsync(uri, data, JsonSerializerOptions, token);
      ScalewayResponse.Email response = await responseMessage.Content.ReadFromJsonAsync<ScalewayResponse.Email>(JsonSerializerOptions, token);
      Logger.LogDebug("Email {id} sent via Scaleway API with status {status}", response.Id, response.Status);
    }
    catch (Exception ex)
    {
      Logger.LogError(ex, "Could not send message via Scaleway API");
    }
  }


  /// <inheritdoc />
  public void Dispose() { }


  /// <summary>
  /// Convert a collection of addresses to a scaleway email addresses
  /// </summary>
  protected List<ScalewayRequest.EmailAddress> Convert(MailAddressCollection addresses)
  {
    return addresses
      .Select(address => new ScalewayRequest.EmailAddress()
      {
        Email = address.Address,
        Name = address.DisplayName
      })
      .ToList();
  }


  /// <summary>
  /// Convert an attachment to a scaleway email attachment
  /// </summary>
  protected ScalewayRequest.EmailAttachment Convert(Attachment attachment)
  {
    byte[] buffer = new byte[8067];
    using MemoryStream memoryStream = new();
    int count;
    while ((count = attachment.ContentStream.Read(buffer, 0, buffer.Length)) > 0)
    {
      memoryStream.Write(buffer, 0, count);
    }
    string base64String = System.Convert.ToBase64String(memoryStream.ToArray());

    return new()
    {
      Name = attachment.Name,
      Type = attachment.ContentType.MediaType,
      Content = base64String
    };
  }
}
