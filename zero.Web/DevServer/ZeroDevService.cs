using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Plugins;

namespace Zero.Web.DevServer
{
  public class ZeroDevService : IHostedService
  {
    IWebHostEnvironment env;
    IOptions<ZeroDevOptions> options;
    ProcessProxy viteProcess;
    ILogger<ZeroDevService> logger;
    PluginResolver pluginResolver;
    string workingDirectory;
    bool isRunning = false;


    public ZeroDevService(IWebHostEnvironment env, IOptions<ZeroDevOptions> options, ILogger<ZeroDevService> logger, IEnumerable<IZeroPlugin> plugins)
    {
      //foreach (IZeroPlugin plugin in plugins)
      //{
      //  string location = Assembly.GetAssembly(plugin.GetType()). ;
      //}
      this.pluginResolver = new PluginResolver(plugins);
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

      // locate npm version and throw if it is not installed
      Version npmVersion = await FindNpmVersion();

      if (npmVersion == null)
      {
        throw new Exception("Please install node+npm to use the zero dev service (https://www.npmjs.com/)");
      }

      // start vite server
      viteProcess = await StartDevServer(options.Value.Port);
      logger.LogInformation("vite listening on: http://localhost:{port}", options.Value.Port);
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

      ProcessProxy process = new ProcessProxy(workingDirectory, "npm").Argument("-v").Capture((value, err) =>
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
    async Task<ProcessProxy> StartDevServer(int port)
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
      ProcessProxy process = new ProcessProxy(workingDirectory, "npm", options.Value.ForwardLog)
        .Argument("run dev")
        .EnvVar("PORT", port.ToString())
        .EnvVar("ZERO_PLUGINS", JsonConvert.SerializeObject(plugins))
        .Capture(CaptureLog);

      await process.RunAsync("localhost:", TimeSpan.FromMinutes(1));

      isRunning = true;

      return process;
    }


    void CaptureLog(string line, bool isError)
    {
      if (!isRunning)
      {
        return;
      }

      if (isError)
      {
        logger.LogWarning(line);
      }
      else
      {
        logger.LogInformation(line);
      }
    }
  }
}
