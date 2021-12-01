namespace zero.Api.Endpoints.Applications;

public class ApplicationBasic : BasicModel<Application>
{
  public string FullName { get; set; }

  public string ImageId { get; set; }

  public Uri[] Domains { get; set; }
}