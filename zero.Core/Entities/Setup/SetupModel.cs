namespace zero.Core.Entities.Setup
{
  public sealed class SetupModel
  {
    public string AppName { get; set; }

    public SetupUserModel User { get; set; }

    public SetupDatabaseModel Database { get; set; }
  }


  public sealed class SetupUserModel
  {
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
  }

  public sealed class SetupDatabaseModel
  {
    public string Url { get; set; }

    public string Name { get; set; }
  }
}
