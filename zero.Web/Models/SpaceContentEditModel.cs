using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class SpaceContentEditModel : ObsoleteEditModel
  {
    public string Alias { get; set; }

    public SpaceContent Model { get; set; }
  }
}
