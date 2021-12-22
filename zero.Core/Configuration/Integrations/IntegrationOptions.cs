using FluentValidation;

namespace zero.Configuration;

public class IntegrationOptions : List<IntegrationType>
{
  public void Add<T>(string alias, string name, string description, string editorAlias = null, List<string> tags = default, string imagePath = null, IValidator validator = null) where T : Integration , new()
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
    if (!typeof(Integration).IsAssignableFrom(type))
    {
      throw new ArgumentException("Type has to inherit the Integration base model", nameof(type));
    }

    Add(new IntegrationType(type)
    {
      Alias = alias,
      Name = name,
      Description = description,
      ImagePath = imagePath,
      Tags = tags,
      Validator = validator,
      Construct = cfg => Activator.CreateInstance(type) as Integration,
      EditorAlias = editorAlias
    });
  }
}
