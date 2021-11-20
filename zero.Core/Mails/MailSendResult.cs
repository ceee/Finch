namespace zero.Mails;

public class MailSendResult
{
  public string Alias { get; set; }

  public DateTimeOffset LastRunDate { get; set; }

  public MailSendResult() { }
}
