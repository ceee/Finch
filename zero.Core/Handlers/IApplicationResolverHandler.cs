using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Handlers
{
  public interface IApplicationResolverHandler : IHandler
  {
    Application Resolve(HttpRequest request, IList<Application> applications);
  }
}
