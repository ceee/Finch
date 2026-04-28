namespace Finch.Mails.Scaleway;

public class ScalewayResponse
{
  public class ListDomains
  {
    public int TotalCount { get; set; }

    public Domain[] Domains { get; set; } = [];
  }

  public class Domain
  {
    public string Name { get; set; }
  }

  public class Email
  {
    public string Id { get; set; }

    public string MessageId { get; set; }

    public string ProjectId { get; set; }

    public string MailFrom { get; set; }

    public string RcptTo { get; set; }

    public string MailRcpt { get; set; }

    public string RcptType { get; set; }

    public string Subject { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string Status { get; set; }

    public string StatusDetails { get; set; }

    public int TryCount { get; set; }
  }
}