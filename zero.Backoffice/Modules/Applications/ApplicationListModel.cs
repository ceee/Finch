namespace zero.Web.Models
{
  public class ApplicationListModel : ListModel
  {
    public string Name { get; set; }

    public string FullName { get; set; }

    public string ImageId { get; set; }

    public string[] Domains { get; set; }

    public bool IsActive { get; set; }
  }
}
