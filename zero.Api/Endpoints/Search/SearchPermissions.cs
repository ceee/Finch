namespace zero.Api.Endpoints.Search;

public class SearchPermissions : PermissionProvider
{
  public const string Group = "zero.search";

  public const string Search = "zero.search.use";


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new(Group, "@search.permissions.search_group");
    group.Permissions.Add(new Permission(Search, "@search.permissions.search"));

    context.AddGroup(group);

    return Task.CompletedTask;
  }
}