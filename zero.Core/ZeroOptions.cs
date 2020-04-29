using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Core
{
  public class ZeroOptions
  {
    public string BackofficePath { get; set; }

    public SectionCollection Sections { get; private set; } = new SectionCollection();

    public ListCollections Lists { get; private set; } = new ListCollections();

    public IList<SettingsGroup> SettingsAreas { get; private set; } = new List<SettingsGroup>();

    public ZeroAuthorizationOptions Authorization { get; private set; } = new ZeroAuthorizationOptions();
  }


  public class ZeroAuthorizationOptions
  {
    public IList<PermissionCollection> Permissions { get; private set; } = new List<PermissionCollection>();
  }
}
