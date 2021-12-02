namespace zero.Pages;

public interface IPageTypeHandler : IHandler
{
  Task<IEnumerable<FlavorConfig>> GetAllowedPageTypes(Application application, IEnumerable<FlavorConfig> registeredTypes, IEnumerable<Page> parents);
}
