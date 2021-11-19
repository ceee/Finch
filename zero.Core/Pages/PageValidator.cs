using FluentValidation;

namespace zero;

public class PageValidator : ZeroValidator<Page>
{
  public PageValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.PageTypeAlias).NotEmpty();
  }
}