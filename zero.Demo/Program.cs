using zero;
using zero.Api;
using zero.Applications;
using zero.Backoffice;
using zero.Backoffice.DevServer;
using zero.Demo;
using zero.Routing;
using zero.Spaces;
using zero.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IApplicationResolverHandler, DevApplicationResolverHandler>();

builder.Services
  .AddZero(builder.Configuration)
  .AddApi()
  .AddBackoffice();

builder.Services.Configure<ZeroDevOptions>(opts =>
{
  opts.Enabled = false;
});

builder.Services.Configure<FlavorOptions>(opts =>
{
  opts.AddSpaceList<TeamMember>("team", "Team", "Members of our team", "fth-users", "spaces.team");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
}

app.UseZero().WithEndpoints(x =>
{
  //x.MapControllerRoute("default", "api/{controller}/{action=Index}/{id?}");
  x.MapControllers();
  x.MapRazorPages();
  x.MapZeroApi();
  x.MapZeroBackoffice();
  x.MapZeroRoutes();
});

//app.UseZeroBackoffice();

app.Run();