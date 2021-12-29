namespace zero.Models;

/// <summary>
/// Entities decorated with this interface are always set to IsActive=true
/// </summary>
public interface IAlwaysActive
{
  /// <summary>
  /// Whether the entity is visible in the frontend
  /// </summary>
  bool IsActive { get; set; }
}