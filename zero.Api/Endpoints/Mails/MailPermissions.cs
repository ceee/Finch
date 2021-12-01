namespace zero.Api.Endpoints.Mails;

public class MailPermissions : PermissionProvider
{
  public const string Create = "zero.settings.mail.create";
  public const string Read = "zero.settings.mail.read";
  public const string Update = "zero.settings.mail.update";
  public const string Delete = "zero.settings.mail.delete";


  public override Task Configure(IPermissionContext context)
  {
    if (!context.TryGetGroup(Constants.Permissions.Groups.Settings, out PermissionGroup group))
    {
      group.Permissions.Add(new Permission("zero.settings.mail", "@settings.application.mails.name")
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