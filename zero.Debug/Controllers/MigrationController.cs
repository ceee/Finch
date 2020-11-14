using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Commerce.Entities;
using zero.Core;
using zero.Core.Api;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Debug.Controllers
{
  public partial class MigrationController : Controller
  {
    private IDocumentStore Raven { get; set; }

    public MigrationController(IDocumentStore raven)
    {
      Raven = raven;
    }
  }
}