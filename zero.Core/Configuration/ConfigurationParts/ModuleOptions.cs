using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero;

public class ModuleOptions : OptionsEnumerable<ModuleType>, IOptionsEnumerable
{
  public void Add<T>(ModuleType<T> moduleType) where T : Module, new()
  {
    Items.Add(ModuleType.Convert(moduleType));
  }


  public void Add<T>(string alias, string name, string description, string icon, string group = null, List<string> tags = null, List<string> disallowedPageTypes = null) where T : Module, new()
  {
    Items.Add(new ModuleType(typeof(T))
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon,
      Group = group,
      Tags = tags ?? new List<string>(),
      DisallowedPageTypes = disallowedPageTypes ?? new List<string>()
    });
  }


  public void Add(Type type, string alias, string name, string description, string icon, string group = null, List<string> tags = null, List<string> disallowedPageTypes = null)
  {
    Items.Add(new ModuleType(type)
    {
      Alias = alias,
      Name = name,
      Description = description,
      Icon = icon,
      Group = group,
      Tags = tags ?? new List<string>(),
      DisallowedPageTypes = disallowedPageTypes ?? new List<string>()
    });
  }
}
