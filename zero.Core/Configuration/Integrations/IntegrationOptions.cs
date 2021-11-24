using FluentValidation;

namespace zero.Configuration;

public class IntegrationOptions : List<IntegrationType>
{
  public void Add<T>(IntegrationType<T> integration) where T : Integration, new()
  {
    Add(IntegrationType.Convert(integration));
  }


  public void Add<T>(string alias, string name, string description, List<string> tags = default, string imagePath = null, IValidator validator = null) where T : Integration, new()
  {
    Add(new IntegrationType(typeof(T))
    {
      Alias = alias,
      Name = name,
      Description = description,
      ImagePath = imagePath,
      Tags = tags,
      Validator = validator
    });
  }


  public void Add(Type type, string alias, string name, string description, List<string> tags = default, string imagePath = null, IValidator validator = null)
  {
    Add(new IntegrationType(type)
    {
      Alias = alias,
      Name = name,
      Description = description,
      ImagePath = imagePath,
      Tags = tags,
      Validator = validator
    });
  }
}
