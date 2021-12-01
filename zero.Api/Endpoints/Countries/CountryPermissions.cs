namespace zero.Api.Endpoints.Countries;

public class CountryPermissions : PermissionProvider
{
  public const string Create = "zero.settings.country.create";
  public const string Read = "zero.settings.country.read";
  public const string Update = "zero.settings.country.update";
  public const string Delete = "zero.settings.country.delete";


  public override Task Configure(IPermissionContext context)
  {
    if (!context.TryGetGroup(Constants.Permissions.Groups.Settings, out PermissionGroup group))
    {
      group.Permissions.Add(new Permission("zero.settings.country", "@settings.application.countries.name")
      {
        Children = new()
        {
          new(Create, "@permission.states.create"),
          new(Read, "@permission.states.read"),
          new(Update, "@permission.states.update"),
          new(Delete, "@permission.states.delete")
        }
      });
    }

    return Task.CompletedTask;
  }
}