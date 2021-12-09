namespace zero.Backoffice.Endpoints.Applications;

public class ApplicationMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<Application, ApplicationPresentation>((source, ctx) => new(), Map);
  }


  protected virtual void Map(Application source, ApplicationPresentation target, IZeroMapperContext ctx)
  {
    target.Id = source.Id;
    target.Alias = source.Alias;
    target.Name = source.Name;
    target.ImageId = source.IconId.Or(source.ImageId);
    target.IsActive = source.IsActive;
  }
}