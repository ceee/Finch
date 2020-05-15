using System.Collections.Generic;

namespace zero.Core.Entities
{
  /// <summary>
  /// An application is a website. zero can host multiple websites at once which share common assets
  /// </summary>
  public class Application : ZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Full company or product name
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Generic contact email. Can be used in various locations
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Image of the application
    /// </summary>
    public string ImageId { get; set; }

    /// <summary>
    /// Simple image of the application (used as favicon)
    /// </summary>
    public string IconId { get; set; }

    /// <summary>
    /// All assigned domains for this application
    /// </summary>
    public string[] Domains { get; set; } = new string[] { };

    /// <summary>
    /// Features which are enabled for this application.
    /// Can be user-defined and affect both backoffice and frontend
    /// </summary>
    public List<string> Features { get; set; } = new List<string>();
  }
}