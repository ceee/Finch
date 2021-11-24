using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class BlueprintController : BackofficeController
  {
    IZeroStore Store;

    public BlueprintController(IZeroStore store)
    {
      Store = store;
    }


    public async Task<ZeroEntity> GetById([FromQuery] string id)
    {
      IAsyncDocumentSession session = Store.Session(global: true);
      return await session.LoadAsync<ZeroEntity>(id);
    }
  }
}
