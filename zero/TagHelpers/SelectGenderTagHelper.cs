using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace zero.TagHelpers;

[HtmlTargetElement("app-gender-select", Attributes = FOR_ATTRIBUTE_NAME)]
public class SelectGenderTagHelper : BaseSelectTagHelper 
{
  ILocalizer _localizer;

  public SelectGenderTagHelper(IHtmlGenerator generator, ILocalizer localizer) : base(generator)
  {
    _localizer = localizer;
  }


  public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
  {
    Items.Add(new SelectListItem(string.Empty, null));

    foreach (Gender gender in Enum.GetValues<Gender>())
    {
      string key = "forms.gender." + gender.ToString().ToLowerInvariant();
      Items.Add(new SelectListItem(_localizer.Text(key), gender.ToString()));
    }

    await base.ProcessAsync(context, output);
  }
}