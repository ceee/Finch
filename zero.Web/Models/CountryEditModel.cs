using System;

namespace zero.Web.Models
{
  public class CountryEditModel : EditModel
  {
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public bool IsPreferred { get; set; }

    public string Code { get; set; }

    public string LanguageId { get; set; }
  }
}
