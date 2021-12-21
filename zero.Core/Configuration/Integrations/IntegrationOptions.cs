using FluentValidation;

namespace zero.Configuration;

public class IntegrationOptions : List<IntegrationType>
{
  public void Add<T>(string alias, string name, string description, string editorAlias = null, List<string> tags = default, string imagePath = null, IValidator validator = null) where T : IntegrationModel, new()
  {
    Add(new IntegrationType(typeof(T))
    {
      Alias = alias,
      Name = name,
      Description = description,
      ImagePath = imagePath,
      Tags = tags,
      Validator = validator,
      Construct = cfg => new T(),
      EditorAlias = editorAlias
    });
  }


  public void Add(Type type, string alias, string name, string description, string editorAlias = null, List<string> tags = default, string imagePath = null, IValidator validator = null)
  {
    Add(new IntegrationType(type)
    {
      Alias = alias,
      Name = name,
      Description = description,
      ImagePath = imagePath,
      Tags = tags,
      Validator = validator,
      Construct = cfg => Activator.CreateInstance(type),
      EditorAlias = editorAlias
    });
  }
}
