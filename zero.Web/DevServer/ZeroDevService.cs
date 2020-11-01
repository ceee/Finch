using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Zero.Web.DevServer
{
  public class ZeroDevService : IHostedService
  {
    IWebHostEnvironment env;
    IOptions<ZeroDevOptions> options;
    ZeroDevProcess viteProcess;
    ILogger<ZeroDevService> logger;
    string workingDirectory;


    public ZeroDevService(IWebHostEnvironment env, IOptions<ZeroDevOptions> options, ILogger<ZeroDevService> logger)
    {
      this.env = env;
      this.options = options;
      this.workingDirectory = options.Value.WorkingDirectory;
      this.logger = logger;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
      // this is a development-time service,
      // therefore no way to enable it in production
      if (!env.IsDevelopment())
      {
        return;
      }

      Version npmVersion = await FindNpmVersion();

      if (npmVersion == null)
      {
        // TODO report and return
      }

      logger.LogDebug("Found npm version {version}", npmVersion);

      viteProcess = await StartDevServer(options.Value.Port);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }


    /// <summary>
    /// Get the node version from the system
    /// </summary>
    async Task<Version> FindNpmVersion()
    {
      Version version = null;

      ZeroDevProcess process = new ZeroDevProcess(workingDirectory, "npm").Argument("-v").Capture((value, err) =>
      {
        if (version == null && !value.Contains("not recognized") && Version.TryParse(value, out Version _version))
        {
          version = _version;
        }
      });

      await process.ExecuteAsync();

      return version;
    }


    /// <summary>
    /// Starts the vite dev server which also support HMR
    /// </summary>
    async Task<ZeroDevProcess> StartDevServer(int port)
    {
      // if the port we want to use is occupied, terminate the process utilizing that port.
      // this occurs when "stop" is used from the debugger and the middleware does not have the opportunity to kill the process
      PidUtils.KillPort((ushort)port, true);

      // get all plugins which need to be passed to vite
      var plugins = new List<dynamic>()
      {
        new
        {
          path = Path.Combine(env.ContentRootPath, "../", "ViteZero.Plugin", "vite.plugin.js")
        }
      };

      // create and run the vite script
      ZeroDevProcess process = new ZeroDevProcess(workingDirectory, "npm", options.Value.ForwardLog)
        .Argument("run dev")
        .EnvVar("PORT", options.Value.Port.ToString())
        .EnvVar("ZERO_PLUGINS", JsonConvert.SerializeObject(plugins));

      await process.RunAsync("running at", TimeSpan.FromMinutes(5));

      return process;
    }
  }
}
