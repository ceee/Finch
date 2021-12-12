namespace zero.Architecture;

public class BlueprintChildInterceptor : Interceptor<ZeroEntity>, IBlueprintInterceptor
{
  string configuredZeroDatabase;


  public BlueprintChildInterceptor(IZeroContext context)
  {
    Gravity = -1;
    configuredZeroDatabase = context.Options.For<RavenOptions>().Database;
  }

  /// <summary>
  /// Only run this interceptor when the database is an app database
  /// </summary>
  public override bool CanHandle(InterceptorParameters args, Type modelType) => args.Context.Store.ResolvedDatabase != configuredZeroDatabase;


  /// <inheritdoc />
  public override Task<InterceptorResult<ZeroEntity>> Deleting(InterceptorParameters args, ZeroEntity model)
  {
    if (model.Blueprint == null)
    {
      return Task.FromResult<InterceptorResult<ZeroEntity>>(default);
    }

    InterceptorResult<ZeroEntity> result = new();
    result.Result = Result<ZeroEntity>.Fail("@blueprint.errors.cannotDeleteChild");
    return Task.FromResult(result);
  }
}