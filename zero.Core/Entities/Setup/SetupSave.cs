namespace zero.Core.Entities.Setup
{
  public sealed class SetupSave
  {
    public string AppName { get; set; }

    public SetupUserSave User { get; set; }

    public SetupDatabaseSave Database { get; set; }

    public SetupSavePart Part { get; set; }
  }


  public sealed class SetupUserSave
  {
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
  }

  public sealed class SetupDatabaseSave
  {
    public string Url { get; set; }

    public string Name { get; set; }
  }


  public enum SetupSavePart
  {
    ValidateUser = 0,
    ValidateApplication = 1,
    ValidateDatabase = 2,
    ValidateFolderAccess = 3,
    WriteSettings = 4,
    SaveData = 5
  }
}
