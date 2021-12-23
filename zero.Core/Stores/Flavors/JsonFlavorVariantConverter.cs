namespace zero.Stores;

public class JsonFlavorVariantConverter : JsonDiscriminatorConverter<ZeroEntity>
{
  protected FlavorOptions Flavors { get; private set; }


  public JsonFlavorVariantConverter(IZeroOptions options) : base("flavor")
  {
    Flavors = options.For<FlavorOptions>();
  }

  protected override Type GetTypeFromDiscriminator(Type requestedType, string discriminator)
  {
    FlavorConfig config = Flavors.Get(requestedType, discriminator);
    return config?.FlavorType ?? requestedType;
  }
}