using System.Net.Mail;

namespace zero.Mails;

public class Mail<T> : Mail where T : class
{
  public T Model { get; set; }
}

public class Mail : MailMessage
{
  public bool IsDeactivated { get; set; }

  public string ViewKey { get; set; }

  public string ViewPath { get; set; }

  public string Tag { get; set; }

  public bool HasView { get; set; } = true;

  public bool IsRendered { get; set; }

  public string Preheader { get; set; }

  public MailPlaceholders Placeholders { get; set; } = new();

  public MailMetadata Metadata { get; set; } = new();
}

public class MailMetadata : Dictionary<string, string>
{

}

public class MailPlaceholders : Dictionary<string, string>
{

}