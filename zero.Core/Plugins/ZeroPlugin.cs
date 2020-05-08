using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core.Plugins
{
  public abstract class ZeroPlugin
  {
    protected SectionCollection Sections { get; private set; }

    protected SpaceCollection Spaces { get; private set; }

    protected RendererCollection Renderers { get; private set; }

    protected IList<SettingsGroup> Settings { get; private set; }

    protected PermissionGroupCollection Permissions { get; private set; }

    protected FeatureCollection Features { get; private set; }

    protected virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
      return services;
    }
  }


  public interface IZeroPlugin
  {

  }
}
