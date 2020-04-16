using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Settings, PermissionsValue.Read)]
  public class SettingsController : BackofficeController
  {
    private ISettingsApi Api { get; set; }

    public SettingsController(IZeroConfiguration config, ISettingsApi api) : base(config)
    {
      Api = api;
    }
  }
}
