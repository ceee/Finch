using System.Text.Json.Serialization;

namespace Mixtape.Mails.Dispatchers.Scaleway;

public class ScalewayRequest
{
  public class SendEmail
  {
    public EmailAddress From { get; set; }

    public List<EmailAddress> To { get; set; } = [];

    public List<EmailAddress> Cc { get; set; } = [];

    public List<EmailAddress> Bcc { get; set; } = [];

    public string Subject { get; set; }

    public string Text { get; set; }

    public string Html { get; set; }

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }

    public List<EmailAttachment> Attachments { get; set; } = [];

    [JsonPropertyName("additional_headers")]
    public List<EmailHeader> AdditionalHeaders { get; set; } = [];
  }


  public class EmailAddress
  {
    public string Email { get; set; }

    public string Name { get; set; }
  }

  public class EmailAttachment
  {
    /// <summary>
    /// Filename of the attachment.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// MIME type of the attachment.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Content of the attachment encoded in base64.
    /// </summary>
    public string Content { get; set; }
  }

  public class EmailHeader
  {
    public string Key { get; set; }

    public string Value { get; set; }
  }
}