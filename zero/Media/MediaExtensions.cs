using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Web;
using System.Numerics;

namespace zero.Media;

public static class MediaExtensions
{
  public static SixLabors.ImageSharp.PointF GetFocalPointF(this MediaFocalPoint focalPoint)
  {
    if (focalPoint == null)
    {
      focalPoint = new() { Left = 0.5m, Top = 0.5m };
    }

    Vector2 center = new((float)focalPoint.Left, (float)focalPoint.Top);
    return ExifOrientationUtilities.Transform(center, Vector2.Zero, Vector2.One, ExifOrientationMode.Unknown);
  }


  public static string CssObjectPosition(this MediaFocalPoint focalPoint)
  {
    string values = string.Empty;

    if (focalPoint != null && (focalPoint.Left != 0.5m || focalPoint.Top != 0.5m))
    {
      Func<decimal, string> round = input => decimal.Round(input, 0, MidpointRounding.ToEven).ToString();
      values = string.Format("{0}% {1}%", round(focalPoint.Left * 100), round(focalPoint.Top * 100));
    }

    return values;
  }

  public static string Resize(this Media media, string preset) => media?.Path.Resize(preset, media?.Metadata?.FocalPoint, true);
  
  public static string Resize(this string path, string preset, bool isZeroMedia) => path.Resize(preset, null, isZeroMedia);

  public static string Resize(this string path, string preset, MediaFocalPoint focalPoint = null, bool isZeroMedia = false)
  {
    if (path.IsNullOrEmpty())
    {
      return string.Empty;
    }

    List<string> parts = path.Split('/').ToList();
    parts.Insert(parts.Count - 1, ":" + preset + StringifyFocalPoint(focalPoint));

    if (parts[0] == "http:" || parts[0] == "https:")
    {
      return string.Join('/', parts);
    }

    if (isZeroMedia)
    {
      // TODO this is bullshit because we need to get the base path from options
      return parts[1] == "media" || parts[0] == "media"
        ? string.Join('/', parts)
        : ("/media" + string.Join('/', parts).EnsureStartsWith('/'));
    }

    return string.Join('/', parts).EnsureStartsWith('/');
  }


  static string StringifyFocalPoint(MediaFocalPoint focalPoint = null)
  {
    string xy = string.Empty;

    if (focalPoint != null && (focalPoint.Left != 0.5m || focalPoint.Top != 0.5m))
    {
      Func<decimal, string> round = input =>
      {
        return decimal.Round(input, 2, MidpointRounding.ToEven).ToString().Replace(',', '.');
      };

      xy = string.Format(":{0},{1}", round(focalPoint.Left), round(focalPoint.Top));
    }

    return xy;
  }
}