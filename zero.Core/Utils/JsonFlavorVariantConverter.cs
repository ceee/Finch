using System.Text.Json;
using System.Text.Json.Serialization;

namespace zero.Utils;

public class JsonFlavorVariantConverter : JsonConverter<ISupportsFlavors>
{
  protected FlavorOptions Flavors { get; private set; }


  public JsonFlavorVariantConverter(IZeroOptions options) : base()
  {
    Flavors = options.For<FlavorOptions>();
  }


  /// <inheritdoc />
  public override bool CanConvert(Type typeToConvert)
  {
    return typeof(ISupportsFlavors).IsAssignableFrom(typeToConvert);
  }


  public override ISupportsFlavors Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    Utf8JsonReader variantReader = reader;
    string flavor = null;

    while (variantReader.Read())
    {
      if (variantReader.TokenType == JsonTokenType.PropertyName)
      {
        string property = variantReader.GetString();

        if (property == "Flavor")
        {
          variantReader.Read();
          flavor = variantReader.GetString();
          break;
        }
      }
    }

    while (reader.Read()) { }

    FlavorConfig config = Flavors.Get(typeToConvert, flavor);

    return config.Construct(config) as ISupportsFlavors;
  }


  public override void Write(Utf8JsonWriter writer, ISupportsFlavors value, JsonSerializerOptions options)
  {
    JsonSerializerOptions newOptions = new(options);
    Type thisType = typeof(JsonFlavorVariantConverter);
    JsonConverter toRemove = newOptions.Converters.FirstOrDefault(x => x.GetType() == thisType);
    newOptions.Converters.Remove(toRemove);
    JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
  }
}
