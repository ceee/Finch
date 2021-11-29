namespace zero.Api.Modules.Countries;

public class CountrySave : SaveModel<Country>
{
  public bool IsPreferred { get; set; }

  public string Code { get; set; }
}