using Microsoft.AspNetCore.Identity;
using zero;
using zero.Identity;
using zero.Raven;
var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddZero(builder.Configuration)
  .AddRavenDb();

builder.Services
  .AddZeroIdentity<AppUser, AppRole>()
  .AddDefaultUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseZero();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


public class AppUser : ZeroIdentityUser { }
public class AppRole : ZeroIdentityRole { }