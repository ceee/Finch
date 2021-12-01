namespace zero.Api.Endpoints.Mails;

public class MailBasic : BasicModel<MailTemplate>
{
  public string Subject { get; set; }
}