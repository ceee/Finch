using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace zero.Debug
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      IHostBuilder builder = Host.CreateDefaultBuilder(args);

      builder.ConfigureWebHostDefaults(webBuilder =>
      {
        webBuilder.UseStartup<Startup>();
      });

      return builder;
    }
  }
}
