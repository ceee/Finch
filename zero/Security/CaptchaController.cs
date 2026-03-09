using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PowCapServer.Abstractions;
using PowCapServer.Models;
using zero.Mvc;

namespace zero.Security;

[ApiController]
[Route("/api/captcha")]
public class CaptchaController(ICaptchaService captchaService) : ZeroController
{
  [HttpPost("challenge")]
  public async Task<IActionResult> PostChallenge()
  {
    ChallengeTokenInfo challengeTokenInfo = await captchaService.CreateChallengeAsync(HttpContext.RequestAborted);
    return Json(challengeTokenInfo);
  }


  [HttpPost("{useCase}/challenge")]
  public async Task<IActionResult> PostChallengeWithUseCase(string useCase)
  {
    ChallengeTokenInfo challengeTokenInfo = await captchaService.CreateChallengeAsync(useCase, HttpContext.RequestAborted);
    return Json(challengeTokenInfo);
  }


  [HttpPost("redeem")]
  public async Task<IActionResult> PostRedeem(ChallengeSolution model)
  {
    if (model == null)
    {
      return BadRequest();
    }

    RedeemChallengeResult result = await captchaService.RedeemChallengeAsync(model).ConfigureAwait(false);
    return Json(result);
  }


  [HttpPost("{useCase}/redeem")]
  public async Task<IActionResult> PostRedeemWithUseCase(string useCase, ChallengeSolution model)
  {
    if (model == null)
    {
      return BadRequest();
    }

    RedeemChallengeResult result = await captchaService.RedeemChallengeAsync(useCase, model).ConfigureAwait(false);
    return Json(result);
  }


  [HttpGet("cap.wasm")]
  public IActionResult GetWasmFile()
  {
    Assembly assembly = typeof(ZeroSecurityModule).GetTypeInfo().Assembly;
    Stream resource = assembly.GetManifestResourceStream("zero.Resources.cap_wasm_bg_0_0_6.wasm");

    if (resource is null)
    {
      return NotFound();
    }

    return File(resource, "application/wasm");
  }


  [HttpGet("cap.widget.js")]
  public IActionResult GetWidgetJsFile()
  {
    Assembly assembly = typeof(ZeroSecurityModule).GetTypeInfo().Assembly;
    Stream resource = assembly.GetManifestResourceStream("zero.Resources.cap_min_0_1_41.js");

    if (resource is null)
    {
      return NotFound();
    }

    return File(resource, "text/javascript");
  }
}