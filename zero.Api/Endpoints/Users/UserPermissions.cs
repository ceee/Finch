namespace zero.Api.Endpoints.Users;

public class UserPermissions : PermissionProvider
{
  public const string Group = "zero.user";

  public const string Create = "zero.user.create";
  public const string Read = "zero.user.read";
  public const string Update = "zero.user.update";
  public const string Delete = "zero.user.delete";


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new(Group, "@user.list");
    group.Permissions.Add(new Permission("zero.user.defaults", "Default permissions")
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