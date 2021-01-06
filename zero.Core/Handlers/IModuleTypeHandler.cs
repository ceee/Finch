using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Handlers
{
  public interface IModuleTypeHandler : IHandler
  {
    IEnumerable<ModuleType> GetAllowedModuleTypes(IApplication application, IEnumerable<ModuleType> registeredTypes, IPage page = default, string[] tags = default);
  }
}
