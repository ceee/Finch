using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class ApplicationEditModel : EditModel
  {
    public string Name { get; set; }

    public Media Image { get; set; }

    public Media Icon { get; set; }

    public string[] Domains { get; set; } = new string[] { };

    public string FullName { get; set; }

    public string Email { get; set; }

    public List<string> Features { get; set; } = new List<string>();
  }
}
