using Microsoft.Extensions.Logging;

namespace zero.Raven;

public class Interceptors : IInterceptors
{
  protected IZeroContext Context { get; set; }
  
  protected IZeroStore Store { get; set; }

  protected Lazy<IEnumerable<IInterceptor>> Registrations { get; set; }

  protected ILogger<IInterceptor> Logger { get; set; }


  public Interceptors(IZeroContext context, IZeroStore store, Lazy<IEnumerable<IInterceptor>> registrations, ILogger<IInterceptor> logger)
  {
    Context = context;
    Store = store;
    Registrations = registrations;
    Logger = logger;
  }


  /// <inheritdoc />
  public InterceptorInstruction<T> ForCreate<T>(T model) where T : ZeroIdEntity, new() => new(this, Store, Context, Registrations, Logger, InterceptorRunType.Create, model);

  /// <inheritdoc />
  public InterceptorInstruction<T> ForUpdate<T>(T model, T previousModel = null) where T : ZeroIdEntity, new() => new(this, Store, Context, Registrations, Logger, InterceptorRunType.Update, model, previousModel);

  /// <inheritdoc />
  public InterceptorInstruction<T> ForDelete<T>(T model) where T : ZeroIdEntity, new() => new(this, Store, Context, Registrations, Logger, InterceptorRunType.Delete, model);
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
  InterceptorInstruction<T> ForUpdate<T>(T model, T previousModel = null) where T : ZeroIdEntity, new();

  /// <summary>
  /// Instruction which can run interceptors before and after deleting an entity
  /// </summary>
  InterceptorInstruction<T> ForDelete<T>(T model) where T : ZeroIdEntity, new();
}