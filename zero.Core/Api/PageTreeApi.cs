using Newtonsoft.Json;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class PageTreeApi : IPageTreeApi
  {
    protected IDocumentStore Raven { get; private set; }


    public PageTreeApi(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public IList<TreeItem> GetChildren(string contentPath, string parent = null) // TODO remove content path as it's only needed to load test data
    {
      string path = System.IO.Path.Combine(contentPath, "Resources/tree" + (parent != null ? "-" + parent.ToString() : String.Empty) + ".debug.json");
      string text = System.IO.File.ReadAllText(path, Encoding.UTF8);

      return JsonConvert.DeserializeObject<List<TreeItem>>(text);
    }
  }


  public interface IPageTreeApi
  {
    /// <summary>
    /// Get all children for the current parent page (or root if empty)
    /// </summary>
    IList<TreeItem> GetChildren(string contentPath, string parent = null);
  }
}
