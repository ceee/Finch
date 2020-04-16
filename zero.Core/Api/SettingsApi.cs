using Microsoft.Extensions.Options;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class SettingsApi : ISettingsApi
  {
    protected ZeroOptions Options { get; private set; }


    public SettingsApi(IOptionsMonitor<ZeroOptions> options)
    {
      Options = options.CurrentValue;
    }


    /// <inheritdoc />
    public IList<SettingsGroup> GetAreas()
    {
      return Options.SettingsAreas;
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
