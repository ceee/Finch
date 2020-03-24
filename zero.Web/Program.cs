using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace zero.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Process ID: " + Process.GetCurrentProcess().Id);
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host
        .CreateDefaultBuilder(args)
        .ConfigureLogging((context, builder) =>
        {
          IConfigurationSection configuration = context.Configuration.GetSection("Logging");
          builder.ClearProviders();

          if (context.HostingEnvironment.IsDevelopment())
          {
            builder.AddConsole();
          }

          if (configuration.GetValue("AsFile", false))
          {
            builder.AddFile(configuration);
          }
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        });
    }
  }
}
