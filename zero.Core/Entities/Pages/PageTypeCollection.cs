using System;
using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class PageTypeCollection : List<PageType>
  {
    public void Add<T>(PageType<T> pageType) where T : Page, new()
    {
      Add(PageType.Convert(pageType));
    }


    public void Add<T>(string alias, string name, string description, string icon, bool allowAsRoot = false, bool allowAllChildrenTypes = false, List<string> allowedChildrenTypes = null) where T : Page, new()
    {
      Add(new PageType(typeof(T))
      {
        Alias = alias,
        Name = name,
        Description = description,
        Icon = icon,
        AllowAsRoot = allowAsRoot,
        AllowAllChildrenTypes = allowAllChildrenTypes,
        AllowedChildrenTypes = allowedChildrenTypes
      });
    }


    public void Add(Type type, string alias, string name, string description, string icon, bool allowAsRoot = false, bool allowAllChildrenTypes = false, List<string> allowedChildrenTypes = null)
    {
      Add(new PageType(type)
      {
        Alias = alias,
        Name = name,
        Description = description,
        Icon = icon,
        AllowAsRoot = allowAsRoot,
        AllowAllChildrenTypes = allowAllChildrenTypes,
        AllowedChildrenTypes = allowedChildrenTypes
      });
    }
  }
}
