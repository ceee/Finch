namespace Mixtape.Raven;

public static class MixtapeIndexExtensions
{
  internal static void RunModifiers<T>(this T index, RavenOptions options) where T : IMixtapeIndexDefinition 
  {
    IEnumerable<RavenIndexModifiersOptions.Modifier> modifiers = options.Indexes.Modifiers.GetAllForType(index.GetType());

    foreach (RavenIndexModifiersOptions.Modifier modifier in modifiers)
    {
      Action<IMixtapeIndexDefinition> action = modifier.Modify.Compile();
      action.Invoke(index);
    }
  }
}
