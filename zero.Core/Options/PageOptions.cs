using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Options
{
  public class PageOptions : ZeroBackofficeCollection<PageType>, IZeroCollectionOptions
  {
    public PageOptions()
    {
      
    }


    public void Add<T>(PageType<T> pageType) where T : Page, new()
    {
      Items.Add(PageType.Convert(pageType));
    }


    public void Add<T>(string alias, string name, string description, string icon, bool allowAsRoot = false, bool allowAllChildrenTypes = false, List<string> allowedChildrenTypes = null) where T : Page, new()
    {
      Items.Add(new PageType(typeof(T))
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
      Items.Add(new PageType(type)
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
