namespace zero.Api.Endpoints.Mails;

public class MailEdit : DisplayModel<MailTemplate>
{
  public string SenderEmail { get; set; }

  public string SenderName { get; set; }

  public string RecipientEmail { get; set; }

  public string Cc { get; set; }

  public string Bcc { get; set; }

  public string Subject { get; set; }

  public string Body { get; set; }

  public string Preheader { get; set; }
}