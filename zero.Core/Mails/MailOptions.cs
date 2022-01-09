namespace zero.Mails;

public class MailOptions
{
  public Func<Mail, string> BuildViewPath { get; set; }
}