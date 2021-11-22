using System;

namespace zero.Backoffice.Modules;

public class CountryEditModel : ObsoleteEditModel
{
  public string Name { get; set; }

  public bool IsPreferred { get; set; }

  public string Code { get; set; }

  public string LanguageId { get; set; }
}