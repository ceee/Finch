namespace zero.Configuration;

public class IntegrationTypeWithStatus
{
  public IntegrationType Type { get; set; }

  public bool IsActive { get; set; }

  public bool IsConfigured { get; set; }

  public string Id { get; set; }
}