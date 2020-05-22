using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Debug.Controllers
{
  public class TestController : Controller
  {
    IAppScope<ITranslationsApi> Api;
    ITranslationsApi CurrentApi;


    public TestController(IAppScope<ITranslationsApi> api, ITranslationsApi currentApi)
    {
      Api = api;
      CurrentApi = currentApi;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
      IList<Translation> global = await Api.Global.GetAll();
      IList<Translation> current = await CurrentApi.GetAll();
      IList<Translation> appTwo = await Api.App("applications.2-A").GetAll();
      IList<Translation> shared = await Api.Shared.GetAll(); 

      return Json(new {
        current,
        global,
        appTwo,
        shared
      });
    }
  }
}