namespace Finch.Raven;

[RavenCollection("Tokens")]
public class SecurityToken : ISupportsDbConventions
{
  public string Id { get; set; }

  public string Key { get; set; }

  public string Token { get; set; }

  public Dictionary<string, string> Metadata { get; set; } = new();
}