namespace zero.Api.Endpoints.Applications;

public class ApplicationEdit : DisplayModel<Application>
{
  public string ImageId { get; set; }

  public string IconId { get; set; }

  public Uri[] Domains { get; set; } = Array.Empty<Uri>();

  public string FullName { get; set; }

  public string Email { get; set; }

  public List<string> Features { get; set; } = new();
}