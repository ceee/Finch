using System.Globalization;

namespace Finch.Localization;

public class CultureChangeMessage : IMessage
{
  public CultureInfo Culture { get; set; }
}