var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();


app.Use(async (context, next) =>
{
  Console.WriteLine("middleware A");
  await next(context);
});

app.UseRouting();

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/zero/api"), branch =>
{
  branch.UsePathBase(new PathString("/zero/api"));
  branch.UseEndpoints(endpoints =>
  {
    endpoints.MapControllers();
  });
});

//app.Map("/api/{**path}", branch =>
//{

//});

//app.MapHealthChecks("/health");

//app.MapGet("weatherforecast", () =>
//{
//  return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//  (
//    DateTime.Now.AddDays(index),
//    Random.Shared.Next(-20, 55),
//    summaries[Random.Shared.Next(summaries.Length)]
//  )).ToArray();
//});

app.Use(async (context, next) =>
{
  Console.WriteLine("middleware B");
  await next(context);
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}