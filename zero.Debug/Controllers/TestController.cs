using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Debug.Models;

namespace zero.Debug.Controllers
{
  public class TestController : Controller
  {
    ITranslationsApi Api;

    IApplicationContext AppContext;


    public TestController(ITranslationsApi api, IApplicationContext appContext)
    {
      Api = api;
      AppContext = appContext;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
      IList<Translation> all = await Api.GetAll();
      IList<Translation> scoped = await Api.Scope(global: true).GetAll();

      return Json(new {
        all,
        scoped
      });
    }
  }
}