using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class SettingsApi : ISettingsApi
  {
    protected IZeroOptions Options { get; set; }


    public SettingsApi(IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public IReadOnlyCollection<SettingsGroup> GetAreas()
    {
      return Options.Settings.GetAllItems();
    }
  }


  public interface ISettingsApi
  {
    /// <summary>
    /// Get settings areas for backoffice display and grouping
    /// </summary>
    IReadOnlyCollection<SettingsGroup> GetAreas();
  }
}
