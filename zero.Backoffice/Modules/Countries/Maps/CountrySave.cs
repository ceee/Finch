using System;

namespace zero.Backoffice.Modules.Countries;

public class CountrySave : SaveModel<Country>
{
  public string Name { get; set; }

  public bool IsPreferred { get; set; }

  public string Code { get; set; }

  public string LanguageId { get; set; }
}