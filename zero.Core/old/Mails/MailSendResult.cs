using Newtonsoft.Json;
using System;
using System.Linq;

namespace zero.Core.Mails
{
  public class MailSendResult
  {
    public string Alias { get; set; }

    public DateTimeOffset LastRunDate { get; set; }

    public MailSendResult() { }
  }
}
