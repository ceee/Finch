using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core
{
  public class ZeroOptions
  {
    public string BackofficePath { get; set; }

    public SectionCollection Sections { get; private set; } = new SectionCollection();

    public SpaceCollection Spaces { get; private set; } = new SpaceCollection();

    public RendererCollection Renderers { get; private set; } = new RendererCollection();

    public IList<SettingsGroup> SettingsAreas { get; private set; } = new List<SettingsGroup>();

    public ZeroAuthorizationOptions Authorization { get; private set; } = new ZeroAuthorizationOptions();
  }


  public class ZeroAuthorizationOptions
  {
    public IList<PermissionCollection> Permissions { get; private set; } = new List<PermissionCollection>();
  }
}
