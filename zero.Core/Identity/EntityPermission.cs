namespace zero.Core.Identity
{
  public class EntityPermission
  {
    /// <summary>
    /// Whether an entity can be read
    /// </summary>
    public bool CanRead { get; set; }

    /// <summary>
    /// Whether an entity can be created
    /// </summary>
    public bool CanCreate { get; set; }

    /// <summary>
    /// Whether an entity can be created in the shared app space
    /// </summary>
    public bool CanCreateShared { get; set; }

    /// <summary>
    /// Whether an entity can be edited or only viewed
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Whether an entity can be deleted
    /// </summary>
    public bool CanDelete { get; set; }

    /// <summary>
    /// Wehther an entity is application aware
    /// </summary>
    public bool IsAppAware { get; set; }

    /// <summary>
    /// Whether an entity can be shared across applications (only for IsAppAware=true)
    /// </summary>
    public bool IsShareable { get; set; }
  }
}
