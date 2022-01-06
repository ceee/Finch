namespace zero.Persistence;

public class IdGenerator
{
  const string CHARS = "abcdefghijklmnopqrstuvwxyz0123456789";

  private static Random random = new();

  /// <summary>
  /// Create a new unique Id
  /// </summary>
  public static string Create(int length = -1)
  {
    if (length < 1)
    {
      length = 12;
    }

    return new string(Enumerable.Repeat(CHARS, length).Select(s => s[random.Next(s.Length)]).ToArray());

    //if (length > 0)
    //{
    //return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
    //  .Replace("/", String.Empty)
    //  .Replace("+", String.Empty)
    //  .Replace("-", String.Empty)
    //  .ToLowerInvariant()
    //  .Substring(0, length);
    //}

    //return Guid.NewGuid().ToString();
  }


  /// <summary>
  /// Creates a simple hash from a string
  /// </summary>
  public static string HashString(string value)
  {
    return value.GetHashCode().ToString().Replace("-", String.Empty);
  }


  /// <summary>
  /// Autofill IDs on an object with [GenerateId] attributes
  /// </summary>
  public static T Autofill<T>(T model)
  {
    // find all Raven Ids
    List<ObjectTraverser.Result<GenerateIdAttribute>> ravenIds = ObjectTraverser.FindAttribute<GenerateIdAttribute>(model);

    // set unset Raven Ids
    foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ravenIds)
    {
      string id = item.Property.GetValue(item.Parent, null) as string;
      if (id.IsNullOrWhiteSpace())
      {
        id = item.Item.Length.HasValue ? Create(item.Item.Length.Value) : Create();
        item.Property.SetValue(item.Parent, id);
      }
    }

    return model;
  }
}
