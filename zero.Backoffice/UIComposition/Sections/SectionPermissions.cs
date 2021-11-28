namespace zero.Backoffice.UIComposition;

public class SectionPermissions : PermissionProvider
{
  protected IEnumerable<IBackofficeSection> Sections { get; private set; }


  public SectionPermissions(IEnumerable<IBackofficeSection> sections)
  {
    Sections = sections;
  }


  public override Task Configure(IPermissionContext context)
  {
    PermissionGroup group = new("zero.sections", "@permissions.collections.sections");

    foreach (IBackofficeSection section in Sections)
    {
      Permission permission = new("zero.sections." + section.Alias, section.Name);

      foreach (IChildBackofficeSection child in section.Children)
      {
        Permission childPermission = new(permission.Key + "." + child.Alias, child.Name);
        permission.Children.Add(childPermission);
      }

      group.Add(permission);
    }

    return Task.CompletedTask;
  }
}