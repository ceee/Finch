using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace zero.Validation;

public static class ModelStateDictionaryExtensions
{
  const char DOT = '.';
  
  public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState, bool convertUnresolvedPropertiesToModel = true, string prefix = "Form", Dictionary<string, string> aliases = default, IEnumerable<string> removePrefixes = default)
  {
    if (result == null || result.IsValid)
    {
      return;
    }

    string[] formProperties = modelState.Keys.ToArray();

    foreach (var error in result.Errors)
    {
      string propertyName = error.PropertyName;

      if (propertyName != null && removePrefixes != null && removePrefixes.Any())
      {
        foreach (var removePrefix in removePrefixes)
        {
          if (propertyName.StartsWith(removePrefix, StringComparison.InvariantCultureIgnoreCase))
          {
            propertyName = propertyName.Substring(removePrefix.Length);
            break;
          }
        }
      }

      string key = prefix.IsNullOrEmpty() ? propertyName : (propertyName.IsNullOrEmpty() ? String.Empty : prefix + DOT + propertyName);
      string message = error.ErrorMessage;
      bool found = formProperties.Contains(key, StringComparer.InvariantCultureIgnoreCase);

      // find property by alias
      if (!found && aliases != null && aliases.ContainsKey(propertyName))
      {
        key = aliases[propertyName];
        key = prefix.IsNullOrEmpty() ? key : (key.IsNullOrEmpty() ? String.Empty : prefix + DOT + key);
        found = formProperties.Contains(key, StringComparer.InvariantCultureIgnoreCase);
      }

      // move property errors to model if they are not part of the form
      if (!found && convertUnresolvedPropertiesToModel)
      {
        key = String.Empty;
      }

      modelState.AddModelError(key, message);
    }
  }
}