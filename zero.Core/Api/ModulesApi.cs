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
    public IList<ModuleType> GetModuleTypes()
    {
      return Options.Modules.GetAllItems().ToList();
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
    /// Get all available module types
    /// </summary>
    IList<ModuleType> GetModuleTypes();

    /// <summary>
    /// Get a specific module type by alias
    /// </summary>
    ModuleType GetModuleType(string alias);
  }
}
