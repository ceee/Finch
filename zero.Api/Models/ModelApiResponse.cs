namespace zero.Api.Models;

public class ModelApiResponse : ApiResponse
{
  public object Model { get; set; }

  public string ChangeToken { get; set; }
}
