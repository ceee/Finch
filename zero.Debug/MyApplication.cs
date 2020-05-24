using FluentValidation;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Validation;

namespace zero.Debug
{
  [Collection("Applications")]
  public class MyApplication : Application
  {
    public string Description { get; set; }
  }

  public class MyApplicationValidator : AbstractValidator<MyApplication>
  {
    public MyApplicationValidator()
    {
      Include(new ApplicationValidator());

      //RuleFor(x => x.Description).NotEmpty();
    }
  }
}
