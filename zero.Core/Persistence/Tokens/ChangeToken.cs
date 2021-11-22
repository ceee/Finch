namespace zero.Persistence;

/// <summary>
/// A change token holds a reference to a database entity
/// This is used to verify change requests for entities in the zero backoffice
/// </summary>
public class ChangeToken : IZeroDbConventions
{
  public string Id { get; set; }

  public string ReferenceId { get; set; }
}