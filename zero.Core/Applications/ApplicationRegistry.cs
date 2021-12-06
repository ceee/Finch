namespace zero.Applications;

public class ApplicationRegistry : IApplicationRegistry
{
  protected IZeroOptions Options { get; set; }


  public ApplicationRegistry(IZeroOptions options)
  {
    Options = options;
  }


  /// <inheritdoc />
  public Task<IEnumerable<ApplicationRegistration>> GetAll()
  {
    return Task.FromResult<IEnumerable<ApplicationRegistration>>(Options.Applications);
  }
}


public interface IApplicationRegistry
{
  /// <summary>
  /// Get all registered applications
  /// </summary>
  Task<IEnumerable<ApplicationRegistration>> GetAll();
}