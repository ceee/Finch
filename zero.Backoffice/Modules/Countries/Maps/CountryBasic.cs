namespace zero.Backoffice.Modules.Countries;

public class CountryBasic : BasicModel<Country>
{
  public bool IsActive { get; set; }

  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}