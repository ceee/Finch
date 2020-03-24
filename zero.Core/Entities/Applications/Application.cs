namespace zero.Core.Entities
{
  /// <summary>
  /// An application is a website. zero can host multiple websites at once which share common assets
  /// </summary>
  public class Application : DatabaseEntity
  {
    /// <summary>
    /// Image of the application
    /// </summary>
    public Media Image { get; set; }

    /// <summary>
    /// Simple image of the application (used of favicon)
    /// </summary>
    public Media Icon { get; set; }

    /// <summary>
    /// All assigned domains for this application
    /// </summary>
    public string[] Domains { get; set; } = new string[] { };
  }
}