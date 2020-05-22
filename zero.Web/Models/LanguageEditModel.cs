namespace zero.Web.Models
{
  public class LanguageEditModel : ObsoleteEditModel
  {
    public string Name { get; set; }

    public string Code { get; set; }

    public bool IsDefault { get; set; }

    public string InheritedLanguageId { get; set; }
  }
}
