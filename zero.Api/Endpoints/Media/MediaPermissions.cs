namespace zero.Api.Endpoints.Media;

public class MediaPermissions : PermissionProvider
{
  public const string Group = "zero.media";

  public const string Create = "zero.settings.media.create";
  public const string Read = "zero.settings.media.read";
  public const string Update = "zero.settings.media.update";
  public const string Delete = "zero.settings.media.delete";


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new(Group, "@media.list");
    group.Permissions.Add(new Permission("zero.media.defaults", "Default permissions")
    {
      Children = new()
      {
        new(Create, "@permission.states.create"),
        new(Read, "@permission.states.read"),
        new(Update, "@permission.states.update"),
        new(Delete, "@permission.states.delete")
      }
    });

    context.AddGroup(group);

    return Task.CompletedTask;
  }
}