// TODO this is only for the backoffice
// not required for API

//using Microsoft.AspNetCore.Mvc;

//namespace zero.Api.Endpoints.Languages;

//public class CulturesController : ZeroApiController
//{
//  protected ICultureService Service { get; set; }

//  protected IZeroOptions Options { get; set; }


//  public CulturesController(ICultureService service, IZeroOptions options)
//  {
//    Service = service;
//    Options = options;
//  }


//  [HttpGet("empty")]
//  [ZeroAuthorize(LanguagePermissions.Create)]
//  public List<Culture> GetAllCultures()
//  {
//    return Service.GetAllCultures();
//  }


//  public List<Culture> GetSupportedCultures()
//  {
//    return Service.GetAllCultures(Options.For<BackofficeOptions>.SupportedLanguages);
//  }
//}