namespace zero.Backoffice.Modules;

public class CountryListModel : ListModel
{
  public string Name { get; set; }

  public bool IsActive { get; set; }

  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}
