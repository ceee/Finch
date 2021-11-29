namespace zero.Api.Modules.Countries;

public class CountryBasic : BasicModel<Country>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}