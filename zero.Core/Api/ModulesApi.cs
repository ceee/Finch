using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class ModulesApi : BackofficeApi, IModulesApi
  {
    protected IZeroOptions Options { get; private set; }

    public ModulesApi(IZeroOptions options, IBackofficeStore store) : base(store)
    {
      Options = options;
    }


    /// <inheritdoc />
    public IList<ModuleType> GetModuleTypes(string[] tags = default)
    {
      IEnumerable<ModuleType> modules = Options.Modules.GetAllItems();

      if (tags?.Length > 0)
      {
        modules = modules.Where(x => x.Tags.Any(t => tags.Contains(t, StringComparer.InvariantCultureIgnoreCase)));
      }

      return modules.ToList();
    }


    /// <inheritdoc />
    public ModuleType GetModuleType(string alias)
    {
      return Options.Modules.GetAllItems().FirstOrDefault(x => x.Alias == alias);
    }
  }


  public interface IModulesApi
  {
    /// <summary>
    /// Get all available module types (can be limited to the passed tags)
    /// </summary>
    IList<ModuleType> GetModuleTypes(string[] tags = default);

    /// <summary>
    /// Get a specific module type by alias
    /// </summary>
    ModuleType GetModuleType(string alias);
  }
}
