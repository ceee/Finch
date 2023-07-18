namespace zero.Mails;

public class MailOptions
{
  public string SenderEmail { get; set; }

  public string SenderName { get; set; }

  public Func<Mail, string> BuildViewPath { get; set; }
}