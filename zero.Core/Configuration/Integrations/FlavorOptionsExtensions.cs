using FluentValidation;

namespace zero.Configuration;

public static class FlavorOptionsExtensions
{
  public static void AddIntegration<T>(this FlavorOptions options, string alias, string name, string description, string editorAlias = null, List<string> tags = default, string imagePath = null, IValidator validator = null) where T : Integration, new()
  {
    options.Add<Integration, T>(new IntegrationType(typeof(T))
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
}
