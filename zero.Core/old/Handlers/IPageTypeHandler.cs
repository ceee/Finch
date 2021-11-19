using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Handlers
{
  public interface IPageTypeHandler : IHandler
  {
    Task<IEnumerable<PageType>> GetAllowedPageTypes(Application application, IEnumerable<PageType> registeredTypes, IEnumerable<Page> parents);
  }
}
