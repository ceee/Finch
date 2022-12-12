using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;
using System.Text.RegularExpressions;

namespace zero.TagHelpers;

[HtmlTargetElement("app-striphtml", Attributes = "text")]
public class StripHtmlTagHelper : TagHelper
{
  [HtmlAttributeName("text")]
  public string Text { get; set; }

  [HtmlAttributeName("maxLength")]
  public int MaxLength { get; set; } = -1;

  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    output.TagName = string.Empty;
    string text = string.Empty;

    if (!String.IsNullOrWhiteSpace(Text))
    {
      text = WebUtility.HtmlDecode(Regex.Replace(Text, "<[^>]*(>|$)", string.Empty).Trim());

      if (MaxLength > 0 && text.Length > MaxLength)
      {
        text = text.Substring(0, MaxLength) + "...";
      }
    }

    output.Content.SetHtmlContent(text);
  }
}
