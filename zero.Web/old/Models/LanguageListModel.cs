using System;

namespace zero.Web.Models
{
  public class LanguageListModel : ListModel
  {
    public string Name { get; set; }

    public string Code { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public string InheritedLanguageId { get; set; }
  }
}
