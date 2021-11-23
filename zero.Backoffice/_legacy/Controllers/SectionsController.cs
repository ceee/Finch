using System.Collections.Generic;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class SectionsController : BackofficeController
  {
    ISectionsApi Api;

    public SectionsController(ISectionsApi api)
    {
      Api = api;
    }


    public IReadOnlyCollection<ISection> GetAll() => Api.GetAll();
  }
}
