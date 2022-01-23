namespace zero.Backoffice.Endpoints.Account;

public class AccountMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<ZeroUser, UserModel>((source, ctx) => new(), Map);
  }


  protected virtual void Map(ZeroUser source, UserModel target, IZeroMapperContext ctx)
  {
    target.Id = source.Id;
    target.CurrentAppId = source.CurrentAppId;
    target.IsSuper = source.IsSuper;
    target.IsActive = source.IsActive;
    target.AvatarId = source.AvatarId;
    target.Name = source.Name;
    target.Email = source.Email;
    target.CreatedDate = source.CreatedDate;
    target.Flavor = source.Flavor;
    target.Culture = source.LanguageId;
  }
}