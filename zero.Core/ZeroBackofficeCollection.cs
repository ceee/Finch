using System;
using System.Collections.Generic;
using System.Text;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core
{
  public class ZeroBackofficeCollection
  {
    public SectionCollection Sections { get; private set; }

    public SpaceCollection Spaces { get; private set; }

    public RendererCollection Renderers { get; private set; }

    public IList<SettingsGroup> Settings { get; private set; }

    public PermissionGroupCollection Permissions { get; private set; }

    public FeatureCollection Features { get; private set; }

    public PageTypeCollection PageTypes { get; private set; }
  }
}
