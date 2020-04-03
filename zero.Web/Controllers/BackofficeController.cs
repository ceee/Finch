using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Controllers
{
  public abstract class BackofficeController : Controller
  {
    protected IZeroConfiguration Configuration { get; set; }


    public BackofficeController(IZeroConfiguration config)
    {
      Configuration = config;
    }
  }
}
