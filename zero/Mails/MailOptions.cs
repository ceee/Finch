namespace zero.Mails;

public class MailOptions
{
  //public string Host { get; set; }

  //public int Port { get; set; }

  //public string Username { get; set; }

  //public string Password { get; set; }

  public string From { get; set; }

  public string FromName { get; set; }

  public string To { get; set; }

  public string ToName { get; set; }

  public string ReplyTo { get; set; }

  public bool Debug { get; set; }

  public string SenderEmail { get; set; }

  public string SenderName { get; set; }

  public PostmarkOptions Postmark { get; set; } = new();

  public Func<Mail, string> BuildViewPath { get; set; }
}