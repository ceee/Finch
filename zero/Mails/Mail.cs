using System.Net.Mail;
using System.Text;

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

  /// <summary>
  /// Set To and From addresses based on values in MailOptions
  /// </summary>
  public Mail For(MailOptions options)
  {
    if (options.To.HasValue())
    {
      To.Add(new MailAddress(options.To, options.ToName));
    }
    if (options.From.HasValue())
    {
      From = new MailAddress(options.From, options.FromName);
    }
    if (options.ReplyTo.HasValue())
    {
      ReplyToList.Add(options.ReplyTo);
    }
    return this;
  }

  /// <summary>
  /// Sets the body as plain text
  /// </summary>
  public string PlainText
  {
    get => Body;
    set
    {
      Body = value;
      IsBodyHtml = false;
      BodyEncoding = Encoding.UTF8;
    }
  }
}

public class MailMetadata : Dictionary<string, string>
{

}

public class MailPlaceholders : Dictionary<string, string>
{

}