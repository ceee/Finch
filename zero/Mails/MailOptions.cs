namespace zero.Mails;

public class MailOptions
{
  //public string Host { get; set; }

  //public int Port { get; set; }

  //public string Username { get; set; }

  //public string Password { get; set; }

  //public string To { get; set; }

  public string SenderEmail { get; set; }

  public string SenderName { get; set; }


  public Func<Mail, string> BuildViewPath { get; set; }
}