using zero.Core;
using zero.Core.Identity;

namespace zero.Web.Defaults
{
  public class SettingsPermissions : PermissionCollection
  {
    public SettingsPermissions()
    {
      Alias = Constants.PermissionCollections.Settings;
      Label = "@permission.collections.settings";
      Description = "@permission.collections.settings_description";

      Items.Add(new Permission(Permissions.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text_default", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Languages, "@settings.system.languages.name", "@settings.system.languages.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Mails, "@settings.system.mails.name", "@settings.system.mails.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text_default", PermissionValueType.CRUD));
      Items.Add(new Permission(Permissions.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", PermissionValueType.CRUD));
    }
  }
}
