namespace zero.Pages;

public interface IPageTypeHandler : IHandler
{
  Task<IEnumerable<PageType>> GetAllowedPageTypes(Application application, IEnumerable<PageType> registeredTypes, IEnumerable<Page> parents);
}
