using System.Collections.Generic;
using System.Net.Mail;
using zero.Core.Entities;

namespace zero.Core.Mails
{
  public class Mail<T> : Mail where T : class
  {
    public T Model { get; set; }
  }

  public class Mail : MailMessage
  {
    public string ViewPath { get; set; }

    public bool HasView { get; set; } = true;

    public bool IsRendered { get; set; }

    public IMailTemplate Template { get; set; }

    public Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();

    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
  }
}
