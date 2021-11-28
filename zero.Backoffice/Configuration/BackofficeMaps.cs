using AutoMapper;

namespace zero.Backoffice.Configuration;

/// <summary>
/// 
/// </summary>
public class BackofficeMaps
{
  public MapperConfiguration MapperConfiguration { get; internal set; }

  internal List<Action<IMapperConfigurationExpression>> Expressions { get; }


  public void Configure(Action<IMapperConfigurationExpression> expression)
  {
    Expressions.Add(expression);
  }


  internal void Run(IMapperConfigurationExpression mapperConfig)
  {
    foreach (var expression in Expressions)
    {
      expression(mapperConfig);
    }
  }
}