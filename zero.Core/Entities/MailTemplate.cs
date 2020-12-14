using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Entities
{
  public class MailTemplate : ZeroEntity, IMailTemplate
  {
    /// <inheritdoc />
    public string Key { get; set; }

    /// <inheritdoc />
    public string SenderEmail { get; set; }

    /// <inheritdoc />
    public string SenderName { get; set; }

    /// <inheritdoc />
    public string RecipientEmail { get; set; }

    /// <inheritdoc />
    public string Cc { get; set; }

    /// <inheritdoc />
    public string Bcc { get; set; }

    /// <inheritdoc />
    public string Subject { get; set; }

    /// <inheritdoc />
    public string Body { get; set; }

    /// <inheritdoc />
    public string Preheader { get; set; }
  }


  [Collection("MailTemplates")]
  public interface IMailTemplate : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Alias which is used to get the template in code
    /// </summary>
    string Key { get; set; }

    /// <summary>
    /// Email address of the sender (overrides email from application)
    /// </summary>
    string SenderEmail { get; set; }

    /// <summary>
    /// Name of the sender (overrides name from application)
    /// </summary>
    string SenderName { get; set; }

    /// <summary>
    /// Email address of the recipient. This is only necessary for templates which do not have a dynamic recipient (e.g. reports).
    /// </summary>
    string RecipientEmail { get; set; }

    /// <summary>
    /// Additional comma-separated emails to send a copy to
    /// </summary>
    string Cc { get; set; }

    /// <summary>
    /// Additional comma-separated emails to send a hidden copy to
    /// </summary>
    string Bcc { get; set; }

    /// <summary>
    /// Email subject (can contain placeholders)
    /// </summary>
    string Subject { get; set; }

    /// <summary>
    /// Email body (can contain placeholders)
    /// </summary>
    string Body { get; set; }

    /// <summary>
    /// Preheader which is displayed in the preview pane (can contain placeholders)
    /// </summary>
    string Preheader { get; set; }
  }
}