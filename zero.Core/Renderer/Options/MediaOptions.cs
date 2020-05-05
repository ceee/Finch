using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Renderer
{
  public class MediaOptions : AbstractFieldOptions
  {
    public uint Limit { get; set; } = 1;

    public decimal MaxFileSize { get; set; } = 10;
    
    public MediaOptionsType Type { get; set; }

    public MediaOptionsDisplay Display { get; set; }

    public List<string> FileExtensions { get; set; } = new List<string>();

    public bool DisallowSelect { get; set; }

    public bool DisallowUpload { get; set; }
  }
}