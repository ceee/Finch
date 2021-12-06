namespace zero.Backoffice.Services;

public class BackofficeSettingGroupPresentation
{
  public string Name { get; set; }

  public IEnumerable<BackofficeSettingPresentation> Items { get; set; }
}

public class BackofficeSettingPresentation
{
  public string Alias { get; set; }

  public string Name { get; set; }

  public string Description { get; set; }

  public string Icon { get; set; }

  public string Url { get; set; }

  public bool IsPlugin { get; set; }
}