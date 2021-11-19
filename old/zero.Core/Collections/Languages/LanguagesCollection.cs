using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace zero.Core.Collections
{
  public class LanguagesCollection : EntityCollection<Language>, ILanguagesCollection
  {
    public LanguagesCollection(ICollectionContext<Language> context) : base(context) { }


    /// <inheritdoc />
    public List<Culture> GetAllCultures(params string[] codes)
    {
      return CultureInfo.GetCultures(CultureTypes.AllCultures)
        .Where(x => !x.Name.IsNullOrWhiteSpace())
        .Select(x => new CultureInfo(x.Name))
        .Where(x => codes.Length > 0 ? codes.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase) : true)
        .OrderBy(x => x.DisplayName)
        .Select(x => new Culture()
        {
          Code = x.Name,
          Name = x.DisplayName
        })
        .ToList();
    }


    /// <inheritdoc />
    protected override void ValidationRules(ZeroValidator<Language> validator)
    {
      validator.RuleFor(x => x.Name).Length(2, 60);
      validator.RuleFor(x => x.Code).Length(2, 10).Culture();
      validator.RuleFor(x => x.IsDefault).Unique(Context.Store).When(x => x.IsDefault).WithMessage("@language.errors.default_unique");
      validator.RuleFor(x => x.IsDefault).ExpectAnyUnique(Context.Store, expectedValue: true).When(x => !x.IsDefault).WithMessage("@language.errors.needs_default");
      validator.RuleFor(x => x.InheritedLanguageId).Must((entity, value) => !entity.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase)).When(x => !x.Id.IsNullOrEmpty()).WithMessage("@language.errors.fallback_invalid");
      validator.RuleFor(x => x.InheritedLanguageId).Equal((string)null).When(x => x.IsDefault).WithMessage("@language.errors.default_no_fallback");
      validator.RuleFor(x => x.InheritedLanguageId).Exists(Context.Store);
      validator.RuleFor(x => x.IsOptional).Equal(false).When(x => x.IsDefault).WithMessage("@language.errors.default_not_optional");
    }
  }


  public interface ILanguagesCollection : IEntityCollection<Language>
  {
    /// <summary>
    /// Get all available cultures
    /// </summary>
    List<Culture> GetAllCultures(params string[] codes);
  }
}
