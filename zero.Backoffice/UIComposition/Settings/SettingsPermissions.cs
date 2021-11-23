using zero.Core;
using zero.Core.Identity;

//namespace zero.Backoffice.UIComposition;

//public class BackofficeUISettingsPermissions : PermissionProvider
//{
//  public BackofficeUISettingsPermissions() : base("@permission.collections.settings") { }

//  static string Prefix = "settings.";

//  public static readonly Permission Create = new(Prefix + "create", "@permission.states.create");
//  public static readonly Permission Read = new(Prefix + "read", "@permission.states.read");
//  public static readonly Permission Update = new(Prefix + "update", "@permission.states.update");
//  public static readonly Permission Delete = new(Prefix + "delete", "@permission.states.delete");


//  /// <inheritdoc />
//  public override IEnumerable<Permission> GetPermissions() => new[] { Create, Read, Update, Delete };
//}


//  public class SettingsPermissions : PermissionCollection
//  {
//    public SettingsPermissions()
//    {
//      Alias = Constants.PermissionCollections.Settings;
//      Label = "@permission.collections.settings";
//      Description = "@permission.collections.settings_description";

//      Items.Add(new Permission(Permissions.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text_default", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Languages, "@settings.system.languages.name", "@settings.system.languages.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Mails, "@settings.system.mails.name", "@settings.system.mails.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text_default", PermissionValueType.CRUD));
//      Items.Add(new Permission(Permissions.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", PermissionValueType.CRUD));
//    }
//  }