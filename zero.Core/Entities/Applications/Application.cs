using System;
using System.Collections.Generic;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// An application is a website. zero can host multiple websites at once which share common assets
  /// </summary>
  public class Application : ZeroEntity, IApplication
  {
    /// <inheritdoc />
    public string Database { get; set; }

    /// <inheritdoc />
    public string FullName { get; set; }

    /// <inheritdoc />
    public string Email { get; set; }

    /// <inheritdoc />
    public string ImageId { get; set; }

    /// <inheritdoc />
    public string IconId { get; set; }

    /// <inheritdoc />
    public Uri[] Domains { get; set; } = new Uri[] { };

    /// <inheritdoc />
    public List<string> Features { get; set; } = new List<string>();
  }


  [Collection("Applications")]
  public interface IApplication : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Raven database name for application data
    /// </summary>
    string Database { get; set; }

    /// <summary>
    /// Full company or product name
    /// </summary>
    string FullName { get; set; }

    /// <summary>
    /// Generic contact email. Can be used in various locations
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// Image of the application
    /// </summary>
    string ImageId { get; set; }

    /// <summary>
    /// Simple image of the application (can be used as favicon)
    /// </summary>
    string IconId { get; set; }

    /// <summary>
    /// All assigned domains for this application
    /// </summary>
    Uri[] Domains { get; set; }

    /// <summary>
    /// Features which are enabled for this application.
    /// Can be user-defined and affect both backoffice and frontend
    /// </summary>
    List<string> Features { get; set; }
  }
}