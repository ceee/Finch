namespace zero.Mails;

public class PostmarkOptions
{
  public string ServerToken { get; set; }

  public string AccountToken { get; set; }

  public string MessageStream { get; set; } = "outbound";
}