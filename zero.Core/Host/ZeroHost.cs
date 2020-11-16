using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Handlers;
using zero.Core.Options;

namespace zero.Core.Host
{
  public interface IZeroHost
  {
    Task<IZeroContext> GetContext(HttpContext httpContext);
    Task Initialize();
  }

  public class ZeroHost : IZeroHost
  {
    protected IZeroStore Store { get; private set; }

    protected IZeroContext Context { get; private set; }

    protected IZeroOptions Options { get; private set; }

    protected ILogger<ZeroHost> Logger { get; private set; }

    protected IHandlerHolder Handler { get; private set; }

    protected IApplicationResolver ApplicationResolver { get; private set; }

    bool IsInitialized;

    readonly ConcurrentBag<IApplication> Applications = new ConcurrentBag<IApplication>();


    public ZeroHost(IZeroStore store, IZeroContext context, IZeroOptions options, ILogger<ZeroHost> logger, IHandlerHolder handler, IApplicationResolver applicationResolver)
    {
      Store = store;
      Options = options;
      Logger = logger;
      Handler = handler;
      ApplicationResolver = applicationResolver;
    }


    public async Task Initialize()
    {
      if (!IsInitialized)
      {
        IsInitialized = true;

        using IAsyncDocumentSession session = Store.OpenCoreSession();

        foreach (IApplication application in await session.Query<IApplication>().ToListAsync())
        {
          Applications.Add(application);
        }
      }
    }


    public async Task<IZeroContext> GetContext(HttpContext httpContext)
    {
      await Context.Resolve(httpContext, Applications);
      return Context;
    }
  }
}
