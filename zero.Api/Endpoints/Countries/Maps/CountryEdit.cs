namespace zero.Api.Endpoints.Countries;

public class CountryEdit : DisplayModel<Country>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}