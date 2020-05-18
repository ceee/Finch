namespace zero.Core.Entities
{
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
  }


  /// <summary>
  /// The modifier displays a small icon (with hover text) next to the main item icon
  /// </summary>
  public class TreeItemModifier
  {
    /// <summary>
    /// Name of the modifier
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Icon to display
    /// </summary>
    public string Icon { get; set; }
  }
}
