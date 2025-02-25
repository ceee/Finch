using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement(Attributes = PREFIX + "*")]
public class ClassTagHelper : TagHelper
{ 
  private const string PREFIX = "app-class:"; 

  [HtmlAttributeName("class")]
  public string Classes { get; set; }

  private IDictionary<string, bool> _classValues;

  [HtmlAttributeName("", DictionaryAttributePrefix = PREFIX)]
  public IDictionary<string, bool> ClassValues
  {
    get => _classValues ??= new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    set => _classValues = value;
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    var items = _classValues.Where(e => e.Value).Select(e => e.Key).ToList();

    if (!string.IsNullOrEmpty(Classes))
    {
      items.Insert(0, Classes);
    }

    if (items.Any())
    {
      var classes = string.Join(" ", items.ToArray());
      output.Attributes.Add("class", classes);
    }
  }
}