namespace zero.Backoffice.Modules.Countries;

public class CountryPermissions : PermissionProvider
{
  public CountryPermissions() : base("@settings.application.countries.name") { }

  public const string Create = "zero.country.create";
  public const string Read   = "zero.country.read";
  public const string Update = "zero.country.update";
  public const string Delete = "zero.country.delete";


  /// <inheritdoc />
  public override IEnumerable<Permission> GetPermissions() => new Permission[] 
  { 
    new(Create, "@permission.states.create"),
    new(Read, "@permission.states.read"),
    new(Update, "@permission.states.update"),
    new(Delete, "@permission.states.delete")
  };
}