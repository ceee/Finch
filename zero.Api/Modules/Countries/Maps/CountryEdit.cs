namespace zero.Api.Modules.Countries;

public class CountryEdit : DisplayModel<Country>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}