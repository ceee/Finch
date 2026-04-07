using FluentValidation;

namespace Finch.Configuration;

public abstract class OptionsType
{
  /// <summary>
  /// Type of the associated model class
  /// </summary>
  public Type ContentType { get; set; }

  /// <summary>
  /// The alias
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// The name of the options type
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Set a validator for the editor
  /// </summary>
  public IValidator Validator { get; set; }
}