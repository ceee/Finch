using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.Processors;
using System.Globalization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace Mixtape.Media.ImageSharp.Processors;

public class LoopWebProcessor : IImageWebProcessor
{
  // <summary>
  /// The command constant for the loop prop.
  /// </summary>
  public const string Loop = "loop";

  /// <summary>
  /// The reusable collection of commands.
  /// </summary>
  private static readonly IEnumerable<string> LoopCommands = [Loop];

  /// <inheritdoc/>
  public IEnumerable<string> Commands { get; } = LoopCommands;

  /// <inheritdoc/>
  public FormattedImage Process(
      FormattedImage image,
      ILogger logger,
      CommandCollection commands,
      CommandParser parser,
      CultureInfo culture)
  {
    if (commands.Contains(Loop))
    {
      SixLabors.ImageSharp.Metadata.ImageMetadata metadata = image.Image.Metadata;
      bool loop = parser.ParseValue<bool>(commands.GetValueOrDefault(Loop), culture);

      if (!loop)
      {
        if (metadata.TryGetWebpMetadata(out WebpMetadata webpMetadata))
        {
          webpMetadata.RepeatCount = 1;
        }
        if (metadata.TryGetGifMetadata(out GifMetadata gifMetadata))
        {
          gifMetadata.RepeatCount = 1;
        }
        if (metadata.TryGetPngMetadata(out PngMetadata pngMetadata))
        {
          pngMetadata.RepeatCount = 1;
        }
      }
    }

    return image;
  }

  /// <inheritdoc/>
  public bool RequiresTrueColorPixelFormat(CommandCollection commands, CommandParser parser, CultureInfo culture) => true;
}
