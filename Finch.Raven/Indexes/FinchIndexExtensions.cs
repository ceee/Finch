namespace Finch.Raven;

public static class FinchIndexExtensions
{
  internal static void RunModifiers<T>(this T index, RavenOptions options) where T : IFinchIndexDefinition 
  {
    IEnumerable<RavenIndexModifiersOptions.Modifier> modifiers = options.Indexes.Modifiers.GetAllForType(index.GetType());

    foreach (RavenIndexModifiersOptions.Modifier modifier in modifiers)
    {
      Action<IFinchIndexDefinition> action = modifier.Modify.Compile();
      action.Invoke(index);
    }
  }
}
