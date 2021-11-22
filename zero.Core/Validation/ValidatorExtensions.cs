using FluentValidation;
using Raven.Client.Documents;
using System.Globalization;

namespace zero.Validation;

public static class ValidatorExtensions
{
  private const char DOT = '.';

  private const char KLAMMERAFFE = '@';

  private static string HEX_REGEX = "#[0-9a-fA-F]{3,8}";

  /// <summary>
  /// Validate a color input as HEX (#aabbccdd or #aabbcc or #abc)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Hex<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Matches(HEX_REGEX).WithMessage("@errors.forms.hex_format");
  }


  /// <summary>
  /// Validate an email
  /// </summary>
  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) =>
    {
      return value.IsNullOrWhiteSpace() || Uri.IsWellFormedUriString(value, UriKind.Absolute);
    }).WithMessage("@errors.forms.url_format");
  }


  /// <summary>
  /// Validate an email
  /// </summary>
  public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) =>
    {
      if (value.IsNullOrWhiteSpace())
      {
        return true;
      }

      int index = value.IndexOf(KLAMMERAFFE);

      if (index < 0 || index == value.Length - 1 || index != value.LastIndexOf(KLAMMERAFFE))
      {
        return false;
      }

      return true;
    }).WithMessage("@errors.forms.email_invalid");
  }


  /// <summary>
  /// Validate one or multiple emails
  /// </summary>
  public static IRuleBuilderOptions<T, string> Emails<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) =>
    {
      if (value.IsNullOrWhiteSpace())
      {
        return true;
      }

      string[] mails = value.Split(',', ';').Select(x => x.Trim()).Where(x => !x.IsNullOrWhiteSpace()).ToArray();

      if (!mails.Any())
      {
        return false;
      }

      foreach (string mail in mails)
      {
        int index = value.IndexOf(KLAMMERAFFE);

        if (index < 0 || index == value.Length - 1 || index != value.LastIndexOf(KLAMMERAFFE))
        {
          return false;
        }
      }

      return true;
    }).WithMessage("@errors.forms.emails_invalid");
  }


  /// <summary>
  /// Check if this value is unique within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> Unique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IZeroStore store) where T : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      bool any = await store.Session().Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(ZeroIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyName.ToPascalCaseId(), value)
        .AnyAsync(cancellation);

      return !any;
    }).WithMessage("@errors.forms.not_unique");
  }


  /// <summary>
  /// Check if this value is at least set once to the expected value within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> ExpectAnyUnique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IZeroStore store, TProperty expectedValue) where T : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      return await store.Session().Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(ZeroIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyName.ToPascalCaseId(), expectedValue)
        .AnyAsync(cancellation);
    }).WithMessage("@errors.forms.not_unique_alone");
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced (appId = shared for shareable entities or appId = current)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T>(this IRuleBuilder<T, string> ruleBuilder, IZeroStore store) where T : ZeroEntity
  {
    return ruleBuilder.Exists<T, T>(store);
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced (appId = shared for shareable entities or appId = current)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T, TReference>(this IRuleBuilder<T, string> ruleBuilder, IZeroStore store) where T : ZeroEntity where TReference : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, id, context, cancellation) =>
    {
      if (id.IsNullOrWhiteSpace())
      {
        return true;
      }

      TReference model = await store.Session().LoadAsync<TReference>(id);
      return model != null;
    }).WithMessage("@errors.forms.reference_notfound");
  }


  /// <summary>
  /// Validates a culture identifier
  /// </summary>
  public static IRuleBuilderOptions<T, string> Culture<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) =>
    {
      if (value.IsNullOrWhiteSpace())
      {
        return true;
      }

      try
      {
        CultureInfo info = CultureInfo.GetCultureInfo(value);
        return info != null && !info.EnglishName.Equals(value, StringComparison.InvariantCultureIgnoreCase);
      }
      catch
      {
        return false;
      }
    }).WithMessage("@errors.forms.culture");
  }




  /// <summary>
  /// Check if this value is unique within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> Unique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IZeroDocumentSession session) where T : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      bool any = await session.Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(ZeroIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyName.ToPascalCaseId(), value)
        .AnyAsync(cancellation);

      return !any;
    }).WithMessage("@errors.forms.not_unique");
  }


  /// <summary>
  /// Check if this value is at least set once to the expected value within a collection
  /// </summary>
  public static IRuleBuilderOptions<T, TProperty> ExpectAnyUnique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IZeroDocumentSession session, TProperty expectedValue) where T : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, value, context, cancellation) =>
    {
      return await session.Advanced.AsyncDocumentQuery<T>()
        .WhereNotEquals(nameof(ZeroIdEntity.Id), entity.Id)
        .WhereEquals(context.PropertyName.ToPascalCaseId(), expectedValue)
        .AnyAsync(cancellation);
    }).WithMessage("@errors.forms.not_unique_alone");
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced (appId = shared for shareable entities or appId = current)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T>(this IRuleBuilder<T, string> ruleBuilder, IZeroDocumentSession session) where T : ZeroEntity
  {
    return ruleBuilder.Exists<T, T>(session);
  }


  /// <summary>
  /// Check if this reference exists and is an entity which can be referenced (appId = shared for shareable entities or appId = current)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Exists<T, TReference>(this IRuleBuilder<T, string> ruleBuilder, IZeroDocumentSession session) where T : ZeroEntity where TReference : ZeroEntity
  {
    return ruleBuilder.MustAsync(async (entity, id, context, cancellation) =>
    {
      if (id.IsNullOrWhiteSpace())
      {
        return true;
      }

      TReference model = await session.LoadAsync<TReference>(id);
      return model != null;
    }).WithMessage("@errors.forms.reference_notfound");
  }
}
