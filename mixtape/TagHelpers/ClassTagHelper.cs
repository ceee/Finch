using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mixtape.TagHelpers;

[HtmlTargetElement(Attributes = Prefix + "*")]
public class ClassTagHelper : TagHelper
{
  private const string Prefix = "app-class:";

  private IDictionary<string, bool> _classValues;

  [HtmlAttributeName("", DictionaryAttributePrefix = Prefix)]
  public IDictionary<string, bool> ClassValues
  {
    get => _classValues ??= new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
    set => _classValues = value;
  }


  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    foreach (KeyValuePair<string, bool> item in _classValues.Where(e => e.Value))
    {
      output.AddClass(item.Key, HtmlEncoder.Default);
    }
  }
}