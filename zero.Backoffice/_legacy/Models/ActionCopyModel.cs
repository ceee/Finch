namespace zero.Web.Models
{
  public class ActionCopyModel
  {
    public string Id { get; set; }

    public string DestinationId { get; set; }

    public bool IncludeDescendants { get; set; }
  }
}
