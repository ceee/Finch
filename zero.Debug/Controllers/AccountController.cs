using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Debug.Models;

namespace zero.Debug.Controllers
{
  public class AccountController : Controller
  {
    [HttpGet]
    public IActionResult Index()
    {
      return View();
    }


    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model)
    {
      model.Posted = true;

      if (ModelState.IsValid)
      {
        var isValid = (model.Username == "username" && model.Password == "password");

        if (!isValid)
        {
          ModelState.AddModelError("", "username or password is invalid");
          return View("Index", model);
        }

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.Username));
        identity.AddClaim(new Claim(ClaimTypes.Name, model.Username));
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberMe });

        return View("Index", model);
      }
      else
      {
        ModelState.AddModelError("", "username or password is blank");
        return View("Index", model);
      }
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync();
      return Redirect("/Account/Index");
    }
  }
}