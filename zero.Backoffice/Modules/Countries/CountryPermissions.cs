namespace zero.Backoffice.Modules.Countries;

public class CountryPermissions : PermissionProvider
{
  public CountryPermissions() : base("@settings.application.countries.name") { }


  /// <inheritdoc />
  public override IEnumerable<string> Requires() => new[] { SectionPermissions.Settings, "zero.settings.country" };


  public const string Create = "zero.settings.country.create";
  public const string Read   = "zero.settings.country.read";
  public const string Update = "zero.settings.country.update";
  public const string Delete = "zero.settings.country.delete";


  /// <inheritdoc />
  public override IEnumerable<Permission> GetPermissions() => new Permission[] 
  { 
    new(Create, "@permission.states.create"),
    new(Read, "@permission.states.read"),
    new(Update, "@permission.states.update"),
    new(Delete, "@permission.states.delete")
  };
}