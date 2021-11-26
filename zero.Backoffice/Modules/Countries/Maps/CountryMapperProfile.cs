namespace zero.Backoffice.Modules.Countries;

public class CountryMapperProfile : ZeroMapperProfile
{
  public CountryMapperProfile()
  {
    CreateMap<Country, CountryBasic>();
    CreateMap<Country, CountryDisplay>();
    CreateMap<CountrySave, Country>();
  }
}
