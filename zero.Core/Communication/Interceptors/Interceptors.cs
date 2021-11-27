using Microsoft.Extensions.Logging;

namespace zero.Communication;

public class Interceptors : IInterceptors
{
  protected IZeroContext Context { get; set; }

  protected IEnumerable<IInterceptor> Registrations { get; set; }

  protected ILogger<IInterceptor> Logger { get; set; }


  public Interceptors(IZeroContext context, IEnumerable<IInterceptor> registrations, ILogger<IInterceptor> logger)
  {
    Context = context;
    Registrations = registrations;
    Logger = logger;
  }


  /// <inheritdoc />
  public InterceptorInstruction<T> ForCreate<T>(T model) where T : ZeroIdEntity, new() => new(this, Context, Registrations, Logger, InterceptorRunType.Create, model);

  /// <inheritdoc />
  public InterceptorInstruction<T> ForUpdate<T>(T model) where T : ZeroIdEntity, new() => new(this, Context, Registrations, Logger, InterceptorRunType.Update, model);

  /// <inheritdoc />
  public InterceptorInstruction<T> ForDelete<T>(T model) where T : ZeroIdEntity, new() => new(this, Context, Registrations, Logger, InterceptorRunType.Delete, model);
}


public interface IInterceptors
{
  /// <summary>
  /// Instruction which can run interceptors before and after a creating an entity
  /// </summary>
  InterceptorInstruction<T> ForCreate<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Instruction which can run interceptors before and after updating an entity
  /// </summary>
  InterceptorInstruction<T> ForUpdate<T>(T model) where T : ZeroIdEntity, new();

  /// <summary>
  /// Instruction which can run interceptors before and after deleting an entity
  /// </summary>
  InterceptorInstruction<T> ForDelete<T>(T model) where T : ZeroIdEntity, new();
}