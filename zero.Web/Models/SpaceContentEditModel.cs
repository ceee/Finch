using System;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Renderer;

namespace zero.Web.Models
{
  public class SpaceContentEditModel : ObsoleteEditModel
  {
    public string Alias { get; set; }

    public SpaceContent Model { get; set; }

    public RendererConfig Config { get; set; }
  }
}
