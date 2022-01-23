using FluentValidation;

namespace zero.Applications;

public class ApplicationStore : EntityStore<Application>, IApplicationStore
{
  public ApplicationStore(IStoreContext context) : base(context)
  {
    Config.Database = Options.For<RavenOptions>().Database;
  }


  //protected override void ValidationRules(ZeroValidator<Application> validator)
  //{
  //  validator.RuleFor(x => x.Name).NotEmpty().Length(2, 50);
  //  validator.RuleFor(x => x.FullName).MaximumLength(120);
  //  validator.RuleFor(x => x.Email).Email().NotEmpty().MaximumLength(120);
  //  validator.RuleFor(x => x.Domains).NotEmpty();
  //}
}

public interface IApplicationStore : IEntityStore<Application>
{
}