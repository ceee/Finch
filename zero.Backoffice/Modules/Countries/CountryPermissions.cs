namespace zero.Backoffice.Modules;

public class CountryPermissions : PermissionProvider
{
  public CountryPermissions() : base("@settings.application.countries.name") { }

  static string Prefix = "countries.";

  public static readonly Permission Create = new(Prefix + "create", "@permission.states.create");
  public static readonly Permission Read   = new(Prefix + "read",   "@permission.states.read");
  public static readonly Permission Update = new(Prefix + "update", "@permission.states.update");
  public static readonly Permission Delete = new(Prefix + "delete", "@permission.states.delete");


  /// <inheritdoc />
  public override IEnumerable<Permission> GetPermissions() => new[] { Create, Read, Update, Delete };
}