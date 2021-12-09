namespace zero.Backoffice.Endpoints.Applications;

public class ApplicationPresentation : ZeroIdEntity
{
  public string Alias { get; set; }

  public string Name { get; set; }

  public bool IsActive { get; set; } 

  public string ImageId { get; set; }
}