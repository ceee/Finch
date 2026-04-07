using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;
using static System.Text.RegularExpressions.Regex;

namespace Finch.TagHelpers;

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

    if (!string.IsNullOrWhiteSpace(Text))
    {
      text = WebUtility.HtmlDecode(Replace(Text, "<[^>]*(>|$)", string.Empty).Trim());

      if (MaxLength > 0 && text.Length > MaxLength)
      {
        text = string.Concat(text.AsSpan(0, MaxLength), "...");
      }
    }

    output.Content.SetHtmlContent(text);
  }
}
