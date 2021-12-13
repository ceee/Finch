using Microsoft.AspNetCore.Mvc.ApplicationModels;
using zero.ApiTry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ZeroRoutesTransformer>();
builder.Services.AddControllers(opts =>
{
  opts.Conventions.Add(new ZeroApiControllerModelConvention("/zero/api"));
});

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapDynamicControllerRoute<ZeroRoutesTransformer>("{**path}");

//app.Use(async (context, next) =>
//{
//  Console.WriteLine("middleware B");
//  await next(context);
//});

app.Run();