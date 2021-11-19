using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Handlers
{
  public interface IModuleTypeHandler : IHandler
  {
    IEnumerable<ModuleType> GetAllowedModuleTypes(Application application, IEnumerable<ModuleType> registeredTypes, Page page = default, string[] tags = default);
  }
}
