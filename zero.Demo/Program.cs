using zero;
using zero.Api;
using zero.Applications;
using zero.Backoffice;
using zero.Backoffice.DevServer;
using zero.Demo;
using zero.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IApplicationResolverHandler, DevApplicationResolverHandler>();

ZeroBuilder zero = builder.Services
      .AddZero(builder.Configuration, opts =>
      {
        opts.Mvc.AddApplicationPart(typeof(Program).Assembly);
      })
      .AddApi()
      .AddBackoffice();

builder.Services.Configure<ZeroDevOptions>(opts =>
{
  opts.Enabled = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
}

app.UseZero().WithEndpoints(x =>
{
  x.MapControllerRoute("default", "api/{controller}/{action=Index}/{id?}");
  x.MapRazorPages();
  x.MapZeroRoutes();
  x.MapZeroBackoffice();
  //x.MapFallbackToController("Index", "Error");
});

app.Run();
