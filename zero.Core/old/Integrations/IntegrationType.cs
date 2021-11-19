using System;
using System.Collections.Generic;
using zero.Core.Options;

namespace zero.Core.Integrations
{
  /// <summary>
  /// An integration is an application part which has a public configuration per app.
  /// It's up to the user to provide functionality.
  /// </summary>
  public class IntegrationType<T> : IntegrationType where T : Integration, new()
  {
    public IntegrationType() : base(typeof(T)) { }
  }

  /// <summary>
  /// An integration is an application part which has a public configuration per app.
  /// It's up to the user to provide functionality.
  /// </summary>
  public class IntegrationType : OptionsType
  {
    /// <summary>
    /// Group integrations by tags
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Optional description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Image of the integration
    /// </summary>
    public string ImagePath { get; set; }


    public IntegrationType(Type type)
    {
      ContentType = type;
    }

    public static IntegrationType Convert<T>(IntegrationType<T> model) where T : Integration, new()
    {
      return new(model.ContentType)
      {
        Alias = model.Alias,
        Name = model.Name,
        Description = model.Description,
        ImagePath = model.ImagePath,
        Tags = model.Tags,
        Validator = model.Validator
      };
    }
  }
}
