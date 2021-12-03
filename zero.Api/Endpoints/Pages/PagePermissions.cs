namespace zero.Api.Endpoints.Pages;

public class PagePermissions : PermissionProvider
{
  public const string Group = "zero.page";

  public const string Create = "zero.page.create";
  public const string Read = "zero.page.read";
  public const string Update = "zero.page.update";
  public const string Delete = "zero.page.delete";


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new(Group, "@page.list");
    group.Permissions.Add(new Permission("zero.page.defaults", "Default permissions")
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