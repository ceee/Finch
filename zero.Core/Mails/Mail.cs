using System.Net.Mail;

namespace zero.Mails;

public class Mail<T> : Mail where T : class
{
  public T Model { get; set; }
}

public class Mail : MailMessage
{
  public bool IsDeactivated { get; set; }

  public string ViewPath { get; set; }

  public bool HasView { get; set; } = true;

  public bool IsRendered { get; set; }

  public string Preheader { get; set; }

  public MailTemplate Template { get; set; }

  public MailPlaceholders Placeholders { get; set; } = new();

  public MailMetadata Metadata { get; set; } = new();
}