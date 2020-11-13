using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using zero.Core.Entities;

namespace zero.Core.Mails
{
  public class Mail : MailMessage
  {
    public IMailTemplate Template { get; set; }

    public Dictionary<string, string> Placeholders { get; private set; } = new Dictionary<string, string>();
  }
}
