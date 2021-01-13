using FluentValidation;
using System;

namespace zero.Core.Options
{
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
    /// Do never return null when it has not been configured yet but the default instance for a settings object
    /// </summary>
    public bool IsAutoActivated { get; set; }

    /// <summary>
    /// Set a validator for the integration editor
    /// </summary>
    public IValidator Validator { get; set; }
  }
}
