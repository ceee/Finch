using System;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core.Plugins
{
  public class ZeroPluginConfiguration : IZeroPluginConfiguration
  {
    public void Configure<T>(Action<T> setupAction) where T : IZeroCollectionOptions
    {
      setupAction(null); // TODO
    }
    //public SectionCollection Sections { get; private set; }

    //public SpaceOptions Spaces { get; private set; }

    //public RendererCollection Renderers { get; private set; }

    //public IList<SettingsGroup> Settings { get; private set; }

    //public PermissionGroupCollection Permissions { get; private set; }

    //public FeatureCollection Features { get; private set; }

    //public PageTypeCollection PageTypes { get; private set; }


    //public ZeroPluginBuilder()
    //{
    //  Sections = new SectionCollection();
    //  Spaces = new SpaceOptions();
    //  Renderers = new RendererCollection();
    //  Settings = new List<SettingsGroup>();
    //  Permissions = new PermissionGroupCollection();
    //  Features = new FeatureCollection();
    //  PageTypes = new PageTypeCollection();
    //}
  }


  public interface IZeroPluginConfiguration
  {
    //SectionCollection Sections { get; }

    //SpaceOptions Spaces { get; }

    //RendererCollection Renderers { get; }

    //IList<SettingsGroup> Settings { get; }

    //PermissionGroupCollection Permissions { get; }

    //FeatureCollection Features { get; }

    //PageTypeCollection PageTypes { get; }

    void Configure<T>(Action<T> setupAction) where T : IZeroCollectionOptions;
  }
}
