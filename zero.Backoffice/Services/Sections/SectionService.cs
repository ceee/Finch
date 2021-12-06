using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace zero.Backoffice.Services;

public class SectionService : ISectionService
{
  protected IOptions<BackofficeOptions> Options { get; set; }

  protected IBackofficeAssetFileSystem FileSystem { get; set; }

  protected ILogger<ISectionService> Logger { get; set; }

  protected IEnumerable<IBackofficeSection> Sections { get; set; }

  protected IEnumerable<ISettingsGroup> SettingsGroups { get; set; }


  public SectionService(IOptions<BackofficeOptions> options, IBackofficeAssetFileSystem fileSystem, ILogger<ISectionService> logger, IEnumerable<IBackofficeSection> sections, IEnumerable<ISettingsGroup> settingsGroups)
  {
    Options = options;
    FileSystem = fileSystem;
    Logger = logger;
    Sections = sections;
    SettingsGroups = settingsGroups;
  }


  /// <inheritdoc />
  public Task<IEnumerable<BackofficeSectionPresentation>> GetSections()
  {
    //bool isSuperUser = AuthenticationApi.IsSuper();
    //IList<Permission> permissions = AuthenticationApi.GetPermissions(Permissions.Sections.PREFIX);

    List<BackofficeSectionPresentation> sections = new();

    foreach (IBackofficeSection section in Sections)
    {
      //if (!isSuperUser && !Permission.CanReadKey(permissions, section.Alias, true))
      //{
      //  continue;
      //}

      string url = Safenames.Alias(section.Alias).EnsureStartsWith('/');

      if (section.Alias == Constants.Sections.Dashboard)
      {
        url = "/";
      }

      BackofficeSectionPresentation backofficeSection = new()
      {
        Alias = section.Alias,
        Name = section.Name,
        Icon = section.Icon,
        Color = section.Color,
        Url = url,
        IsExternal = false
      };

      List<BackofficeSectionPresentation> children = new();

      foreach (IBackofficeSection child in section.Children)
      {
        children.Add(new()
        {
          Alias = child.Alias,
          Name = child.Name,
          Url = backofficeSection.Url.EnsureEndsWith('/') + Safenames.Alias(child.Alias),
          IsExternal = false
        });
      }

      backofficeSection.Children = children;

      sections.Add(backofficeSection);
    }

    return Task.FromResult<IEnumerable<BackofficeSectionPresentation>>(sections);
  }


  /// <inheritdoc />
  public Task<IEnumerable<BackofficeSettingGroupPresentation>> GetSettingsAreas()
  {
    //bool isSuperUser = AuthenticationApi.IsSuper();
    //IList<Permission> permissions = AuthenticationApi.GetPermissions(Permissions.Settings.PREFIX);

    List<BackofficeSettingGroupPresentation> groups = new();

    //bool hasIntegrations = Options.For<IntegrationOptions>().Any();

    foreach (SettingsGroup group in SettingsGroups)
    {
      List<BackofficeSettingPresentation> areas = new();

      foreach (SettingsArea area in group.Areas)
      {
        //if (!isSuperUser && !Permission.CanReadKey(permissions, area.Alias, true))
        //{
        //  continue;
        //}
        //if (area.Alias == Constants.Settings.Integrations && !hasIntegrations)
        //{
        //  continue;
        //}

        //bool isPlugin = !(area is InternalSettingsArea);

        BackofficeSettingPresentation settingsArea = new()
        {
          Alias = area.Alias,
          Name = area.Name,
          Description = area.Description,
          Icon = area.Icon,
          Url = Constants.Sections.Settings.EnsureStartsWith('/') + Safenames.Alias(area.Alias).EnsureStartsWith('/'),
          IsPlugin = true
        };

        areas.Add(settingsArea);
      }

      if (areas.Count > 0)
      {
        groups.Add(new()
        {
          Name = group.Name,
          Items = areas
        });
      }
    }

    return Task.FromResult<IEnumerable<BackofficeSettingGroupPresentation>>(groups);
  }
}


public interface ISectionService
{
  /// <summary>
  /// Get all registered backoffice sections
  /// </summary>
  Task<IEnumerable<BackofficeSectionPresentation>> GetSections();

  /// <summary>
  /// Get all registered backoffice settings
  /// </summary>
  Task<IEnumerable<BackofficeSettingGroupPresentation>> GetSettingsAreas();
}