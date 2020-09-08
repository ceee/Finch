using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Validation
{
  public class UniqueValidator : PropertyValidator
  {
    public UniqueValidator() : base("@errors.forms.not_unique")
    {

    }

    protected override bool IsValid(PropertyValidatorContext context)
    {
      throw new NotImplementedException();
    }
  }
}
