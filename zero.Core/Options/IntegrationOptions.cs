using FluentValidation;
using System;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Integrations;

namespace zero.Core.Options
{
  public class IntegrationOptions : ZeroBackofficeCollection<IntegrationType>, IZeroCollectionOptions
  {
    public void Add<T>(IntegrationType<T> integration) where T : Integration, new()
    {
      Items.Add(IntegrationType.Convert(integration));
    }


    public void Add<T>(string alias, string name, string description, bool isAutoActivated = false, List<string> tags = default, string imagePath = null, IValidator validator = null) where T : Integration, new()
    {
      Items.Add(new IntegrationType(typeof(T))
      {
        Alias = alias,
        Name = name,
        Description = description,
        IsAutoActivated = isAutoActivated,
        ImagePath = imagePath,
        Tags = tags,
        Validator = validator
      });
    }


    public void Add(Type type, string alias, string name, string description, bool isAutoActivated = false, List<string> tags = default, string imagePath = null, IValidator validator = null)
    {
      Items.Add(new IntegrationType(type)
      {
        Alias = alias,
        Name = name,
        Description = description,
        IsAutoActivated = isAutoActivated,
        ImagePath = imagePath,
        Tags = tags,
        Validator = validator
      });
    }
  }
}
