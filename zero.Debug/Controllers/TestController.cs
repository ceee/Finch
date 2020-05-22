using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Debug.Controllers
{
  public class TestController : Controller
  {
    IAppScope<ITranslationsApi> Api;
    ITranslationsApi CurrentApi;

    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;


    public TestController(IAppScope<ITranslationsApi> api, ITranslationsApi currentApi, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
      _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
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


    

    [HttpGet]
    public IActionResult Routes()
    {

      var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.ToList();
      if (routes != null && routes.Any())
      {
        return Json(routes);
      }
      return Json(new string [0]);
    }
  }

  internal class RouteModel
  {
    public string Name { get; set; }
    public string Template { get; set; }
  }
}