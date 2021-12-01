namespace zero.Api.Endpoints.Applications;

public class ApplicationMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Application, ApplicationBasic>((source, ctx) => new(), Map);
    mapper.Define<Application, ApplicationEdit>((source, ctx) => new(), Map);
    mapper.Define<ApplicationSave, Application>((source, ctx) => new(), Map);
  }


  protected virtual void Map(Application source, ApplicationBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
    target.ImageId = source.ImageId;
    target.Domains = source.Domains;
    target.FullName = source.FullName;
  }

  protected virtual void Map(Application source, ApplicationEdit target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.ImageId = source.ImageId;
    target.IconId = source.IconId;
    target.Domains = source.Domains;
    target.FullName = source.FullName;
    target.Email = source.Email;
    target.Features = source.Features;
}

  protected virtual void Map(ApplicationSave source, Application target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    target.ImageId = source.ImageId;
    target.IconId = source.IconId;
    target.Domains = source.Domains;
    target.FullName = source.FullName;
    target.Email = source.Email;
    target.Features = source.Features;
  }
}