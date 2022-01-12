namespace zero.Api.Endpoints.PageModules;

public class PageModulePermissions : PermissionProvider
{
  public const string Group = "zero.pagemodules";

  public const string Create = "zero.pagemodules.create";
  public const string Read = "zero.pagemodules.read";
  public const string Update = "zero.pagemodules.update";
  public const string Delete = "zero.pagemodules.delete";


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new(Group, "@module.list");
    group.Permissions.Add(new Permission("zero.pagemodules.defaults", "Default permissions")
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