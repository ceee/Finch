namespace zero.Pages;

public interface IModuleTypeHandler : IHandler
{
  IEnumerable<PageModuleType> GetAllowedModuleTypes(Application application, IEnumerable<PageModuleType> registeredTypes, Page page = default, string[] tags = default);
}