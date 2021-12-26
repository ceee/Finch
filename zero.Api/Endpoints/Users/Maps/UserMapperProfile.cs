namespace zero.Api.Endpoints.Users;

public class UserMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<ZeroUser, UserBasic>((source, ctx) => new(), Map);
  }


  protected virtual void Map(ZeroUser source, UserBasic target, IZeroMapperContext ctx)
  {
    target.Id = source.Id;
    target.Name = source.Name;
    target.Email = source.Email;
    target.IsActive = source.IsActive;
    target.AvatarId = source.AvatarId;
  }
}