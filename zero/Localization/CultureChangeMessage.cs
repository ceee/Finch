using System.Globalization;

namespace zero.Localization;

public class CultureChangeMessage : IMessage
{
  public CultureInfo Culture { get; set; }
}