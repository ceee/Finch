using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class PageEditModel<T> : EditModel<T> where T : Page
  {
    public PageType PageType { get; set; }

    public ListResult<Revision> Revisions { get; set; }

    public List<string> Urls { get; set; } = new();
  }
}
