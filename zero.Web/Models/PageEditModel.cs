using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class PageEditModel<T> : EditModel<T> where T : IPage
  {
    public PageType PageType { get; set; }

    public ListResult<Revision> Revisions { get; set; }
  }
}
