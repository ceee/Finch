using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class PermissionsApi : IPermissionsApi
  {
    protected IZeroOptions Options { get; set; }


    public PermissionsApi (IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public IList<PermissionCollection> GetAll()
    {
      IList<PermissionCollection> result = new List<PermissionCollection>();

      PermissionCollection permissionSections = new PermissionCollection()
      {
        Alias = Constants.PermissionCollections.Sections,
        Label = "@permission.collections.sections",
        Description = "@permission.collections.sections_description"
      };

      permissionSections.Items.Add(new Permission(Permissions.Sections.Dashboard, "@sections.item.dashboard", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Pages, "@sections.item.pages", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Spaces, "@sections.item.spaces", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Media, "@sections.item.media", null, PermissionValueType.Boolean));
      permissionSections.Items.Add(new Permission(Permissions.Sections.Settings, "@sections.item.settings", null, PermissionValueType.Boolean));

      result.Add(permissionSections);

      // TODO add back spaces

      if (Options.Backoffice.Spaces.Count > 0)
      {
        PermissionCollection permissionSpaces = new PermissionCollection()
        {
          Alias = Constants.PermissionCollections.Spaces,
          Label = "@permission.collections.spaces",
          Description = "@permission.collections.spaces_description"
        };

        foreach (Space space in Options.Backoffice.Spaces)
        {
          permissionSpaces.Items.Add(new Permission(Permissions.Spaces.PREFIX + space.Alias, space.Name, null, PermissionValueType.ReadWrite));
        }

        result.Add(permissionSpaces);
      }

      PermissionCollection permissionSettings = new PermissionCollection()
      {
        Alias = Constants.PermissionCollections.Settings,
        Label = "@permission.collections.settings",
        Description = "@permission.collections.settings_description"
      };

      permissionSettings.Items.Add(new Permission(Permissions.Settings.Updates, "@settings.system.updates.name", "@settings.system.updates.text_default", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Applications, "@settings.system.applications.name", "@settings.system.applications.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Users, "@settings.system.users.name", "@settings.system.users.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Languages, "@settings.system.languages.name", "@settings.system.languages.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Countries, "@settings.system.countries.name", "@settings.system.countries.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Translations, "@settings.system.translations.name", "@settings.system.translations.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Logging, "@settings.system.logs.name", "@settings.system.logs.text", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.Plugins, "@settings.plugins.installed.name", "@settings.plugins.installed.text_default", PermissionValueType.ReadWrite));
      permissionSettings.Items.Add(new Permission(Permissions.Settings.CreatePlugin, "@settings.plugins.create.name", "@settings.plugins.create.text", PermissionValueType.ReadWrite));

      result.Add(permissionSettings);

      return result;
    }
  }


  public interface IPermissionsApi
  {
    /// <summary>
    /// Get all available permissions to choose from
    /// </summary>
    IList<PermissionCollection> GetAll();
  }
}
