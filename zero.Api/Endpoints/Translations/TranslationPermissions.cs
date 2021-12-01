namespace zero.Api.Endpoints.Translations;

public class TranslationPermissions : PermissionProvider
{
  public const string Create = "zero.settings.translation.create";
  public const string Read = "zero.settings.translation.read";
  public const string Update = "zero.settings.translation.update";
  public const string Delete = "zero.settings.translation.delete";


  public override Task Configure(IPermissionContext context)
  {
    if (!context.TryGetGroup(Constants.Permissions.Groups.Settings, out PermissionGroup group))
    {
      group.Permissions.Add(new Permission("zero.settings.translation", "@settings.application.translations.name")
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