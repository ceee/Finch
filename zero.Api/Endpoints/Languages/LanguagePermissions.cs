namespace zero.Api.Endpoints.Languages;

public class LanguagePermissions : PermissionProvider
{
  public const string Create = "zero.settings.language.create";
  public const string Read = "zero.settings.language.read";
  public const string Update = "zero.settings.language.update";
  public const string Delete = "zero.settings.language.delete";


  public override Task Configure(IPermissionContext context)
  {
    if (!context.TryGetGroup(Constants.Permissions.Groups.Settings, out PermissionGroup group))
    {
      group.Permissions.Add(new Permission("zero.settings.language", "@settings.application.languages.name")
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