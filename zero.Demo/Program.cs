using zero;
using zero.Api;
using zero.Applications;
using zero.Architecture;
using zero.Backoffice;
using zero.Backoffice.DevServer;
using zero.Demo;
using zero.Localization;
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

  opts.Configure<Country>(x => x.CanUseWithoutFlavors = false);
  opts.Add<Country, EuropeanCountry>("eu_country", "EU country", "A country within the European Union", "fth-globe");
  opts.Add<Country, AmericanCountry>("usa_country", "USA", "A country in the United States", "fth-flag");
});

builder.Services.Configure<BlueprintOptions>(opts =>
{
  opts.Add<Country>();
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


public class EuropeanCountry : Country
{
  public string EuName { get; set; }
}

public class AmericanCountry : Country
{
  public string State { get; set; }
}