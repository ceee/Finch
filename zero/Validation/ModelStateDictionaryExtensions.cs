using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
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
        foreach (string removePrefix in removePrefixes)
        {
          if (propertyName.StartsWith(removePrefix, StringComparison.InvariantCultureIgnoreCase))
          {
            propertyName = propertyName.Substring(removePrefix.Length);
            break;
          }
        }
      }

      string key = prefix.IsNullOrEmpty() ? propertyName : (propertyName.IsNullOrEmpty() ? string.Empty : prefix + DOT + propertyName);
      string message = error.ErrorMessage;
      bool found = formProperties.Contains(key, StringComparer.InvariantCultureIgnoreCase);

      // find property by alias
      if (!found && aliases != null && aliases.ContainsKey(propertyName))
      {
        key = aliases[propertyName];
        key = prefix.IsNullOrEmpty() ? key : (key.IsNullOrEmpty() ? string.Empty : prefix + DOT + key);
        found = formProperties.Contains(key, StringComparer.InvariantCultureIgnoreCase);
      }

      // move property errors to model if they are not part of the form
      if (!found && convertUnresolvedPropertiesToModel)
      {
        key = string.Empty;
      }

      modelState.AddModelError(key, message);
    }
  }
  
  
  public static void AddToModelState<T>(this Result<T> result, ModelStateDictionary modelState, bool convertUnresolvedPropertiesToModel = true, string prefix = "Form", Dictionary<string, string> aliases = default, IEnumerable<string> removePrefixes = default)
  {
    if (result == null || result.IsSuccess)
    {
      return;
    }

    string[] formProperties = modelState.Keys.ToArray();

    foreach (var error in result.Errors)
    {
      string propertyName = error.Property;

      if (propertyName != null && removePrefixes != null && removePrefixes.Any())
      {
        foreach (string removePrefix in removePrefixes)
        {
          if (propertyName.StartsWith(removePrefix, StringComparison.InvariantCultureIgnoreCase))
          {
            propertyName = propertyName.Substring(removePrefix.Length);
            break;
          }
        }
      }

      string key = prefix.IsNullOrEmpty() ? propertyName : (propertyName.IsNullOrEmpty() ? string.Empty : prefix + DOT + propertyName);
      string message = error.Message;
      bool found = formProperties.Contains(key, StringComparer.InvariantCultureIgnoreCase);

      // find property by alias
      if (!found && aliases != null && aliases.ContainsKey(propertyName))
      {
        key = aliases[propertyName];
        key = prefix.IsNullOrEmpty() ? key : (key.IsNullOrEmpty() ? string.Empty : prefix + DOT + key);
        found = formProperties.Contains(key, StringComparer.InvariantCultureIgnoreCase);
      }

      // move property errors to model if they are not part of the form
      if (!found && convertUnresolvedPropertiesToModel)
      {
        key = string.Empty;
      }

      modelState.AddModelError(key, message);
    }
  }
  

  public static void AddToModelState(this IdentityResult result, ModelStateDictionary modelState)
  {
    if (result == null || result.Succeeded)
    {
      return;
    }

    foreach (IdentityError error in result.Errors)
    {
      modelState.AddModelError(string.Empty, error.Description);
    }
  }
}