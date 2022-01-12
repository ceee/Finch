using System.Text.Json;
using System.Text.Json.Serialization;

namespace zero.Stores;

internal class JsonModuleTypeConverter : JsonDiscriminatorConverter<PageModule>
{
  Type _converterType = typeof(JsonModuleTypeConverter);

  readonly IZeroOptions _options;


  public JsonModuleTypeConverter(IZeroOptions options) : base(nameof(PageModule.ModuleTypeAlias))
  {
    _options = options;
  }


  protected override Type GetTypeFromDiscriminator(Type requestedType, string discriminator)
  {
    PageModuleType config = _options.For<PageModuleOptions>().FirstOrDefault(x => x.Alias.Equals(discriminator, StringComparison.InvariantCultureIgnoreCase));
    return config?.FlavorType ?? requestedType;
  }


  protected override JsonSerializerOptions CreateOptions(JsonSerializerOptions baseOptions)
  {
    JsonSerializerOptions newOptions = base.CreateOptions(baseOptions);
    JsonConverter toRemove = newOptions.Converters.FirstOrDefault(x => x.GetType() == _converterType);
    newOptions.Converters.Remove(toRemove);
    return newOptions;
  }
}