using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class SettingsApi : ISettingsApi
  {
    protected IZeroOptions Options { get; private set; }


    public SettingsApi(IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public IList<SettingsGroup> GetAreas()
    {
      return Options.Backoffice.Settings;
    }
  }


  public interface ISettingsApi
  {
    /// <summary>
    /// Get settings areas for backoffice display and grouping
    /// </summary>
    IList<SettingsGroup> GetAreas();
  }
}
