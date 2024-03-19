using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.Processors;
using System.Globalization;

namespace zero.Media.ImageSharp.Processors;

public class StripMetadataWebProcessor : IImageWebProcessor
{
  // <summary>
  /// The command constant for quality.
  /// </summary>
  public const string Strip = "strip";

  /// <summary>
  /// The reusable collection of commands.
  /// </summary>
  private static readonly IEnumerable<string> StripCommands
      = new[] { Strip };

  /// <inheritdoc/>
  public IEnumerable<string> Commands { get; } = StripCommands;

  /// <inheritdoc/>
  public FormattedImage Process(
      FormattedImage image,
      ILogger logger,
      CommandCollection commands,
      CommandParser parser,
      CultureInfo culture)
  {
    if (commands.Contains(Strip))
    {
      bool strip = parser.ParseValue<bool>(commands.GetValueOrDefault(Strip), culture);

      if (strip)
      {
        image.Image.Metadata.ExifProfile = null;
        image.Image.Metadata.XmpProfile = null;
        image.Image.Metadata.IptcProfile = null;
        image.Image.Metadata.IccProfile = null;
      }
    }

    return image;
  }

  /// <inheritdoc/>
  public bool RequiresTrueColorPixelFormat(CommandCollection commands, CommandParser parser, CultureInfo culture) => true;
}
