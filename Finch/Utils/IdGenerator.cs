using System.Text.Json;

namespace Finch.Utils;

public class IdGenerator
{
  const string CHARS_az09 = "abcdefghijklmnopqrstuvwxyz0123456789";
  const string CHARS_azAZ09x = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-@#.:!?*";
  const string CHARS_x09 = "0123456789";

  private static Random random = new();

  /// <summary>
  /// Create a new unique Id
  /// </summary>
  public static string Create(int length = -1, Charset charset = Charset.az09)
  {
    if (length < 1)
    {
      length = 12;
    }

    string chars = charset == Charset.az09 ? CHARS_az09 : charset == Charset.x09 ? CHARS_x09 : CHARS_azAZ09x;

    return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

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
    return GetStableHashCode(value).ToString().Replace("-", String.Empty);
  }


  /// <summary>
  /// Creates a simple hash from a string
  /// </summary>
  public static string HashObject(params object[] values)
  {
    return GetStableHashCode(JsonSerializer.Serialize(values)).ToString().Replace("-", String.Empty);
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


  static int GetStableHashCode(string str)
  {
    unchecked
    {
      int hash1 = 5381;
      int hash2 = hash1;

      for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
      {
        hash1 = ((hash1 << 5) + hash1) ^ str[i];
        if (i == str.Length - 1 || str[i + 1] == '\0')
          break;
        hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
      }

      return hash1 + (hash2 * 1566083941);
    }
  }


  public enum Charset
  {
    /// <summary>
    /// a-z, 0-9
    /// </summary>
    az09 = 0,
    /// <summary>
    /// a-z, A-Z, 0-9, _-@#.:!?*
    /// </summary>
    azAZ09x = 1,
    /// <summary>
    /// 0-9
    /// </summary>
    x09 = 2
  }
}