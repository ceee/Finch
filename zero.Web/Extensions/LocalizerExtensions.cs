using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using zero.Core.Services;

namespace zero.Web.Extensions
{
  public static class LocalizerExtensions
  {
    public static IHtmlContent Html(this ILocalizer localizer, string key)
    {
      string value = localizer.Text(key);

      HtmlContentBuilder builder = new();
      builder.SetHtmlContent(value);
      return builder;
    }


    public static IHtmlContent Html(this ILocalizer localizer, string key, Dictionary<string, string> tokens)
    {
      string value = localizer.Text(key, tokens);

      HtmlContentBuilder builder = new();
      builder.SetHtmlContent(value);
      return builder;
    }
  }
}
