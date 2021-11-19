using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Core.Validation
{
  public class PageValidator : ZeroValidator<Page>
  {
    public PageValidator()
    {
      RuleFor(x => x.Name).NotEmpty();
      RuleFor(x => x.PageTypeAlias).NotEmpty();
    }
  }
}