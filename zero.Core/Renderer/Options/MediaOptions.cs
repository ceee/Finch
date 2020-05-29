using System.Collections.Generic;

namespace zero.Core.Renderer
{
  public class MediaOptions : AbstractFieldInputOptions
  {
    public uint Limit { get; set; } = 1;

    public decimal MaxFileSize { get; set; } = 10;
    
    public MediaOptionsType Type { get; set; }

    public MediaOptionsDisplay Display { get; set; }

    public List<string> FileExtensions { get; set; } = new List<string>();

    public bool DisallowSelect { get; set; }

    public bool DisallowUpload { get; set; }
  }


  public enum MediaOptionsDisplay
  {
    Default = 0,
    Big = 1,
    Grid = 2
  }


  public enum MediaOptionsType
  {
    All = 0,
    Image = 1,
    Video = 2,
    Document = 3,
    Other = 99
  }
}