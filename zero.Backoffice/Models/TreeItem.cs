namespace zero.Backoffice.Models;

/// <summary>
/// Represents an item in a tree
/// </summary>
public class TreeItem
{
  /// <summary>
  /// Id of the item
  /// </summary>
  public string Id { get; set; }

  /// <summary>
  /// Parent id of the item
  /// </summary>
  public string ParentId { get; set; }

  /// <summary>
  /// Sort order
  /// </summary>
  public uint Sort { get; set; }

  /// <summary>
  /// Name of the item
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Displays a description on hover
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Icon to display alongside the name
  /// </summary>
  public string Icon { get; set; }

  /// <summary>
  /// Whether this item is open in case it contains children
  /// </summary>
  public bool IsOpen { get; set; }

  /// <summary>
  /// Displays a small icon (with hover text) next to the main item icon
  /// </summary>
  public TreeItemModifier Modifier { get; set; }

  /// <summary>
  /// Whether this item has children
  /// </summary>
  public bool HasChildren { get; set; }

  /// <summary>
  /// Count of children
  /// </summary>
  public int ChildCount { get; set; }

  /// <summary>
  /// Whether this item is published or not
  /// </summary>
  public bool IsInactive { get; set; }

  /// <summary>
  /// Whether to display the item icon with a dashed line
  /// </summary>
  public bool IsDashed { get; set; }

  /// <summary>
  /// Whether to show actions menu. This will only work when the onActionsRequested cb is implemented in the component
  /// </summary>
  public bool HasActions { get; set; }

  /// <summary>
  /// Output an additional count value.
  /// </summary>
  public int? CountOutput { get; set; }
}