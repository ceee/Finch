using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public class RecycleBinController : BackofficeController
  {
    IRecycleBinApi Api;

    public RecycleBinController(IRecycleBinApi api)
    {
      Api = api;
    }


    public async Task<ListResult<IRecycledEntity>> GetByQuery([FromQuery] RecycleBinListQuery query) => await Api.GetByQuery(query);


    public async Task<int> GetCountByOperation([FromQuery] string operationId) => await Api.GetCountByOperation(operationId);


    [HttpDelete]
    public async Task<EntityResult<IRecycledEntity>> Delete([FromQuery] string id) => await Api.Delete(id);


    [HttpDelete]
    public async Task<EntityResult<IRecycledEntity>> DeleteByGroup([FromQuery] string group) => await Api.DeleteByGroup(group);

  }
}
