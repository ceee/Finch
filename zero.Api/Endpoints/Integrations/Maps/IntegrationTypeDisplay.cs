namespace zero.Api.Endpoints.Integrations;

public class IntegrationTypeDisplay
{
  public string EditorAlias { get; set; }

  public string Alias { get; set; }

  public string Name { get; set; }

  public List<string> Tags { get; set; } = new();

  public string Description { get; set; }

  public string ImagePath { get; set; }

  public bool IsActivated { get; set; }

  public bool IsConfigured { get; set; }

  public string ModelId { get; set; }
}
