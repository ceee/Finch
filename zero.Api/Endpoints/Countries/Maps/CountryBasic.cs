namespace zero.Api.Endpoints.Countries;

public class CountryBasic : BasicModel<Country>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}