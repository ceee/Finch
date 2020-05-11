using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Mapper;

namespace zero.Core.Plugins
{
  public abstract class ZeroPlugin
  {
    public SectionCollection Sections { get; private set; } = new SectionCollection();

    public SpaceCollection Spaces { get; private set; } = new SpaceCollection();

    public RendererCollection Renderers { get; private set; } = new RendererCollection();

    public IList<SettingsGroup> Settings { get; private set; } = new List<SettingsGroup>();

    public PermissionGroupCollection Permissions { get; private set; } = new PermissionGroupCollection();

    public FeatureCollection Features { get; private set; } = new FeatureCollection();

    public IMapper Mapper { get; private set; }

    protected virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
      return services;
    }
  }


  public interface IZeroPlugin
  {

  }
}
