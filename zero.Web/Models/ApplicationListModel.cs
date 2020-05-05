namespace zero.Web.Models
{
  public class ApplicationListModel : ListModel
  {
    public string Name { get; set; }

    public string Image { get; set; }

    public string[] Domains { get; set; }

    public bool IsActive { get; set; }
  }
}
