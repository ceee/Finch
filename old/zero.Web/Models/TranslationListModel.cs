using System;

namespace zero.Web.Models
{
  public class TranslationListModel : ListModel
  {
    public string Key { get; set; }

    public string Value { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
  }
}
