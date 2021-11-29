namespace zero.Api.Modules.Countries;

public class CountryDisplay : DisplayModel<Country>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}