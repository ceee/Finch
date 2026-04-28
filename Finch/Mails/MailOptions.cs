using Finch.Mails.Dispatchers.Postmark;
using Finch.Mails.Dispatchers.Scaleway;

namespace Finch.Mails;

public class MailOptions
{
  public string From { get; set; }

  public string FromName { get; set; }

  public string To { get; set; }

  public string ToName { get; set; }

  public string ReplyTo { get; set; }

  public bool Debug { get; set; }

  public string SenderEmail { get; set; }

  public string SenderName { get; set; }

  public PostmarkOptions Postmark { get; set; }

  public ScalewayOptions Scaleway { get; set; }

  public Func<Mail, string> BuildViewPath { get; set; }
}