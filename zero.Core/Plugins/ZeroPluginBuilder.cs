using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Mapper;

namespace zero.Core.Plugins
{
  public class ZeroPluginBuilder : IZeroPluginBuilder
  {
    public SectionCollection Sections { get; private set; }

    public SpaceCollection Spaces { get; private set; }

    public RendererCollection Renderers { get; private set; };

    public IList<SettingsGroup> Settings { get; private set; }

    public PermissionGroupCollection Permissions { get; private set; }

    public FeatureCollection Features { get; private set; }

    public PageTypeCollection PageTypes { get; private set; }

    public IMapper Mapper { get; private set; }


    public ZeroPluginBuilder(IMapper mapper)
    {
      Mapper = mapper;
      Sections = new SectionCollection();
      Spaces = new SpaceCollection();
      Renderers = new RendererCollection();
      Settings = new List<SettingsGroup>();
      Permissions = new PermissionGroupCollection();
      Features = new FeatureCollection();
      PageTypes = new PageTypeCollection();
      //Mapper = mapper;
    }
  }


  public interface IZeroPluginBuilder
  {
    SectionCollection Sections { get; }

    SpaceCollection Spaces { get; }

    RendererCollection Renderers { get; }

    IList<SettingsGroup> Settings { get; }

    PermissionGroupCollection Permissions { get; }

    FeatureCollection Features { get; }

    PageTypeCollection PageTypes { get; }

    IMapper Mapper { get; }
  }
}
