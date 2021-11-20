namespace zero.Communication;

public interface IModuleTypeHandler : IHandler
{
  IEnumerable<ModuleType> GetAllowedModuleTypes(Application application, IEnumerable<ModuleType> registeredTypes, Page page = default, string[] tags = default);
}
