namespace zero.Mails;

[RavenCollection("MailTemplates")]
public class MailTemplate : ZeroEntity, IAlwaysActive
{
  /// <summary>
  /// Email address of the sender (overrides email from application)
  /// </summary>
  public string SenderEmail { get; set; }

  /// <summary>
  /// Name of the sender (overrides name from application)
  /// </summary>
  public string SenderName { get; set; }

  /// <summary>
  /// Email address of the recipient. This is only necessary for templates which do not have a dynamic recipient (e.g. reports).
  /// </summary>
  public string RecipientEmail { get; set; }

  /// <summary>
  /// Additional comma-separated emails to send a copy to
  /// </summary>
  public string Cc { get; set; }

  /// <summary>
  /// Additional comma-separated emails to send a hidden copy to
  /// </summary>
  public string Bcc { get; set; }

  /// <summary>
  /// Email subject (can contain placeholders)
  /// </summary>
  public string Subject { get; set; }

  /// <summary>
  /// Email body (can contain placeholders)
  /// </summary>
  public string Body { get; set; }

  /// <summary>
  /// Preheader which is displayed in the preview pane (can contain placeholders)
  /// </summary>
  public string Preheader { get; set; }
}