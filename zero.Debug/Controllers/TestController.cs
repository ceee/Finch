using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;

namespace zero.Debug.Controllers
{
  public class TestController : Controller
  {
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;


    public TestController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
      _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }


    [HttpGet]
    public IActionResult Services()
    {
      List<object> items = new List<object>();

      foreach (var service in Startup.Services)
      {
        if (!service.ServiceType.FullName.StartsWith("zero.") && (service.ImplementationType == null || !service.ImplementationType.FullName.StartsWith("zero.")))
        {
          continue;
        }

        items.Add(new
        {
          name = service.ServiceType.FullName,
          lifetime = service.Lifetime,
          instance = service.ImplementationType?.FullName
        });
      }

      return Json(items);
    }


    [HttpGet]
    public IActionResult Things([FromServices] IValidator<Application> appValidator)
    {
      return Ok();
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