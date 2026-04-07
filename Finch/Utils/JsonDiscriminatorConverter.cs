using System.Text.Json;
using System.Text.Json.Serialization;

namespace Finch.Utils;

public abstract class JsonDiscriminatorConverter<T> : JsonConverter<T> where T : class, new()
{
  Type _type;
  string _discriminatorPropertyName;

  public JsonDiscriminatorConverter(string discriminatorPropertyName) : base()
  {
    _discriminatorPropertyName = discriminatorPropertyName;
    _type = GetType();
  }


  protected abstract Type GetTypeFromDiscriminator(Type requestedType, string discriminator);


  /// <inheritdoc />
  public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);


  /// <inheritdoc />
  public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    Utf8JsonReader variantReader = reader;
    string discriminatorValue = null;

    while (variantReader.Read())
    {
      if (variantReader.TokenType == JsonTokenType.PropertyName)
      {
        string property = variantReader.GetString();

        if (property != null && property.Equals(_discriminatorPropertyName, StringComparison.InvariantCultureIgnoreCase))
        {
          variantReader.Read();
          discriminatorValue = variantReader.GetString();
          break;
        }
      }
    }

    Type childType = GetTypeFromDiscriminator(typeToConvert, discriminatorValue);
    Type resolvedType = childType ?? typeToConvert;

    return JsonSerializer.Deserialize(ref reader, resolvedType, CreateOptions(options)) as T;
  }


  /// <inheritdoc />
  public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, value, value.GetType(), CreateOptions(options));
  }


  protected virtual JsonSerializerOptions CreateOptions(JsonSerializerOptions baseOptions)
  {
    JsonSerializerOptions newOptions = new(baseOptions);
    JsonConverter toRemove = newOptions.Converters.FirstOrDefault(x => x.GetType() == _type);
    newOptions.Converters.Remove(toRemove);
    return newOptions;
  }
}
