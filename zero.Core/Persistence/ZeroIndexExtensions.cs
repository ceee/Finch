namespace zero.Persistence;

public static class ZeroIndexExtensions
{
  internal static void RunModifiers<T>(this T index, IZeroOptions options) where T : IZeroIndexDefinition 
  {
    IEnumerable<RavenIndexModifiersOptions.Modifier> modifiers = options.Raven.Indexes.Modifiers.GetAllForType(index.GetType());

    foreach (RavenIndexModifiersOptions.Modifier modifier in modifiers)
    {
      Action<IZeroIndexDefinition> action = modifier.Modify.Compile();
      action.Invoke(index);
    }
  }
}
