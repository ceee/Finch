using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Handlers;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class ModulesApi : BackofficeApi, IModulesApi
  {
    protected IZeroOptions Options { get; private set; }

    protected IHandlerHolder Handler { get; private set; }

    public ModulesApi(IZeroOptions options, IBackofficeStore store, IHandlerHolder handler) : base(store)
    {
      Options = options;
      Handler = handler;
    }


    /// <inheritdoc />
    public async Task<IList<ModuleType>> GetModuleTypes(string[] tags = default, string pageId = default)
    {
      IEnumerable<ModuleType> types = Options.Modules.GetAllItems();
      List<ModuleType> modules = types.ToList();
      IPage page = null;

      if (!pageId.IsNullOrEmpty())
      {
        page = await GetById<IPage>(pageId);
      }

      if (tags?.Length > 0)
      {
        modules = types.Where(x => x.Tags.Any(t => tags.Contains(t, StringComparer.InvariantCultureIgnoreCase))).ToList();
      }

      IModuleTypeHandler handler = Handler.Get<IModuleTypeHandler>();

      // if there is no registered handler we just allow all page types
      if (handler == null)
      {
        return modules;
      }

      return handler.GetAllowedModuleTypes(Backoffice.Context.Application, types, page, tags)?.ToList() ?? new();
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
    Task<IList<ModuleType>> GetModuleTypes(string[] tags = default, string pageId = default);

    /// <summary>
    /// Get a specific module type by alias
    /// </summary>
    ModuleType GetModuleType(string alias);
  }
}
