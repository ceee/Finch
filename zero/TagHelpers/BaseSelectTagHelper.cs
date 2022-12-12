using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;

namespace zero.TagHelpers;

public abstract class BaseSelectTagHelper : TagHelper
{
  protected const string FOR_ATTRIBUTE_NAME = "asp-for";
  protected bool AllowMultiple;
  protected ICollection<string> CurrentValues;

  protected BaseSelectTagHelper(IHtmlGenerator generator)
  {
    Generator = generator;
  }


  /// <summary>
  /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="SelectTagHelper"/>'s output.
  /// </summary>
  protected IHtmlGenerator Generator { get; }

  /// <summary>
  /// Gets the <see cref="Rendering.ViewContext"/> of the executing view.
  /// </summary>
  [HtmlAttributeNotBound]
  [ViewContext]
  public ViewContext ViewContext { get; set; }

  /// <summary>
  /// An expression to be evaluated against the current model.
  /// </summary>
  [HtmlAttributeName(FOR_ATTRIBUTE_NAME)]
  public ModelExpression For { get; set; }

  /// <summary>
  /// A collection of <see cref="SelectListItem"/> objects used to populate the &lt;select&gt; element with
  /// &lt;optgroup&gt; and &lt;option&gt; elements.
  /// </summary>
  public List<SelectListItem> Items { get; set; } = new List<SelectListItem>();

  /// <summary>
  /// The name of the &lt;input&gt; element.
  /// </summary>
  /// <remarks>
  /// Passed through to the generated HTML in all cases. Also used to determine whether <see cref="For"/> is
  /// valid with an empty <see cref="ModelExpression.Name"/>.
  /// </remarks>
  public string Name { get; set; }


  /// <inheritdoc />
  public override void Init(TagHelperContext context)
  {
    if (context == null)
    {
      throw new ArgumentNullException(nameof(context));
    }

    if (For == null)
    {
      // Informs contained elements that they're running within a targeted <select/> element.
      context.Items[typeof(SelectTagHelper)] = null;
      return;
    }

    // Base allowMultiple on the instance or declared type of the expression to avoid a
    // "SelectExpressionNotEnumerable" InvalidOperationException during generation.
    // Metadata.IsEnumerableType is similar but does not take runtime type into account.
    var realModelType = For.ModelExplorer.ModelType;
    AllowMultiple = typeof(string) != realModelType && typeof(IEnumerable).IsAssignableFrom(realModelType);
    CurrentValues = Generator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, AllowMultiple);

    // Whether or not (not being highly unlikely) we generate anything, could update contained <option/>
    // elements. Provide selected values for <option/> tag helpers.
    //var currentValues = _currentValues == null ? null : new CurrentValues(_currentValues);
    //context.Items[typeof(SelectTagHelper)] = currentValues;
  }


  public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
  {
    output.TagName = "select";

    // Pass through attribute that is also a well-known HTML attribute. Must be done prior to any copying
    // from a TagBuilder.
    if (Name != null)
    {
      output.CopyHtmlAttribute(nameof(Name), context);
    }

    // Ensure GenerateSelect() _never_ looks anything up in ViewData.
    var items = Items ?? Enumerable.Empty<SelectListItem>();

    // Ensure Generator does not throw due to empty "fullName" if user provided a name attribute.
    IDictionary<string, object> htmlAttributes = null;
    if (string.IsNullOrEmpty(For.Name) && string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) && !string.IsNullOrEmpty(Name))
    {
      htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
      {
        { "name", Name },
      };
    }

    var tagBuilder = Generator.GenerateSelect(
        ViewContext,
        For.ModelExplorer,
        optionLabel: null,
        expression: For.Name,
        selectList: items,
        currentValues: CurrentValues,
        allowMultiple: AllowMultiple,
        htmlAttributes: htmlAttributes);

    if (tagBuilder != null)
    {
      output.MergeAttributes(tagBuilder);
      if (tagBuilder.HasInnerHtml)
      {
        output.PostContent.AppendHtml(tagBuilder.InnerHtml);
      }
    }

    output.PreElement.AppendHtml("<div class=\"form-select\">");
    // TODO use iconoptions + taghelper
    output.PostElement.AppendHtml("<span class=\"-icon\"><svg class=\"app-icon\" width=\"18\" height=\"18\" xmlns=\"http://www.w3.org/2000/svg\" stroke-width=\"2\" data-symbol=\"chevron-down\"><use xlink:href=\"/assets/icons/feather.svg#chevron-down\"></use></svg></span>");
    output.PostElement.AppendHtml("</div>");

    return Task.CompletedTask;
  }
}