using Microsoft.Extensions.Logging;

namespace zero.Raven;

public class InterceptorInstruction<T> where T : ZeroIdEntity, new()
{
  public Guid Guid { get; private set; }

  public InterceptorRunType Runtype { get; private set; }

  public T Model { get; private set; }

  public T PreviousModel { get; private set; }

  public Type ModelType { get; private set; }

  public Result<T> Result { get; private set; }

  protected IZeroContext Context { get; private set; }
  
  protected IZeroStore Store { get; private set; }

  protected Lazy<IEnumerable<IInterceptor>> Interceptors { get; private set; }

  protected Dictionary<IInterceptor, InterceptorParameters> InterceptorCache { get; private set; } = new();

  protected IInterceptors InterceptorHandler { get; private set; }

  protected ILogger<IInterceptor> Logger { get; private set; }

  protected Func<IInterceptor, bool> InterceptorFilter { get; private set; } = x => true;


  internal InterceptorInstruction(IInterceptors interceptors, IZeroStore store, IZeroContext context, Lazy<IEnumerable<IInterceptor>> registrations, ILogger<IInterceptor> logger, InterceptorRunType runtype, T model, T previousModel = null)
  {
    InterceptorHandler = interceptors;
    Store = store;
    Context = context;
    Guid = Guid.NewGuid();
    Logger = logger;
    Runtype = runtype;
    Model = model;
    PreviousModel = previousModel;
    Interceptors = registrations;

    ModelType = model.GetType();
  }


  /// <summary>
  /// Custom interceptor filter for this instruction (the CanHandle() method for each interceptor is still activated)
  /// </summary>
  public void Filter(Func<IInterceptor, bool> predicate)
  {
    InterceptorFilter = predicate;
  }


  /// <summary>
  /// Run all interceptors (in order) which can handle the given type.
  /// Depending on the action any of the following methods on the interceptor is called: Creating(), Updating(), Deleting().
  /// If any of the interceptors returns a result the operation is cancelled and this result is returned.
  /// </summary>
  public async Task<bool> Start(IRavenOperations operations)
  {
    foreach (IInterceptor interceptor in GetInterceptors())
    {
      InterceptorParameters parameters = new()
      {
        Context = Context,
        Store =  Store,
        Properties = new(),
        Interceptors = InterceptorHandler,
        Operations = operations,
        PreviousModel = PreviousModel
      };

      if (!interceptor.CanHandle(parameters, ModelType))
      {
        continue;
      }
    

      Logger.LogDebug("Run interceptor {interceptor} for {type}:{operation}", interceptor.Name, ModelType, Runtype);

      InterceptorResult<ZeroIdEntity> result = (await HandleBefore(interceptor, parameters)) ?? new();
      result.InterceptorHash = IdGenerator.Create(32);

      InterceptorCache.Add(interceptor, parameters);

      // we cancel all further interceptors if a result is available and return this instead
      if (result.Result != null)
      {
        Result = result.Result.ConvertTo(result.Result.Model as T);
        return false;
      }

      // the Continue task will cancel all further interceptors
      if (!result.Continue)
      {
        break;
      }
    }

    return true;
  }


  /// <summary>
  /// Run all interceptors (in order) which can handle the given type.
  /// Depending on the action any of the following methods on the interceptor is called: Created(), Updated(), Deleted().
  /// The parameters which are returned from the Start() operation are passed to the methods.
  /// </summary>
  public async Task Complete()
  {
    foreach ((IInterceptor interceptor, InterceptorParameters parameters) in InterceptorCache)
    {
      await HandleAfter(interceptor, parameters);
    }
  }


  protected IEnumerable<IInterceptor> GetInterceptors()
  {
    return Interceptors.Value.Where(InterceptorFilter).OrderByDescending(x => x.Gravity);
  }


  /// <summary>
  /// Proxy for handling methods on an interceptor
  /// </summary>
  protected Task<InterceptorResult<ZeroIdEntity>> HandleBefore(IInterceptor interceptor, InterceptorParameters parameters) => Runtype switch
  {
    InterceptorRunType.Create => interceptor.Creating(parameters, Model),
    InterceptorRunType.Update => interceptor.Updating(parameters, Model),
    InterceptorRunType.Delete => interceptor.Deleting(parameters, Model),
    _ => throw new NotImplementedException()
  };


  /// <summary>
  /// Proxy for handling methods on an interceptor
  /// </summary>
  protected Task HandleAfter(IInterceptor interceptor, InterceptorParameters parameters) => Runtype switch
  {
    InterceptorRunType.Create => interceptor.Created(parameters, Model),
    InterceptorRunType.Update => interceptor.Updated(parameters, Model),
    InterceptorRunType.Delete => interceptor.Deleted(parameters, Model),
    _ => throw new NotImplementedException()
  };
}