namespace zero.Api.Endpoints.Spaces;

public class SpacePermissions : PermissionProvider
{
  public const string Group = "zero.space";

  public const string Create = "zero.space.create";
  public const string Read = "zero.space.read";
  public const string Update = "zero.space.update";
  public const string Delete = "zero.space.delete";


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new(Group, "@space.list");
    group.Permissions.Add(new Permission("zero.space.defaults", "Default permissions")
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