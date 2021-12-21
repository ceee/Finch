namespace zero.Api.Endpoints.Integrations;

public class IntegrationMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<IntegrationType, IntegrationTypeDisplay>((source, ctx) => new(), Map);
  }


  protected virtual void Map(IntegrationType source, IntegrationTypeDisplay target, IZeroMapperContext ctx)
  {
    target.Description = source.Description;
    target.Name = source.Name;
    target.Alias = source.Alias;
    target.EditorAlias = source.EditorAlias;
    target.ImagePath = source.ImagePath;
    target.Tags = source.Tags;
  }
}