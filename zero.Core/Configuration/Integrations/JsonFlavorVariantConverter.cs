namespace zero.Configuration;

public class JsonIntegrationTypeVariantConverter : JsonDiscriminatorConverter<ZeroEntity>
{
  protected IntegrationOptions Integrations { get; private set; }


  public JsonIntegrationTypeVariantConverter(IZeroOptions options) : base("typeAlias")
  {
    Integrations = options.For<IntegrationOptions>();
  }

  protected override Type GetTypeFromDiscriminator(Type requestedType, string discriminator)
  {
    IntegrationType type = Integrations.FirstOrDefault(x => x.Alias == discriminator);
    return type?.ModelType ?? requestedType;
  }
}