using System.Linq;
using zero.Commerce.Entities;
using zero.Core.Renderer;
using zero.Debug.Models;
using zero.TestData;

namespace zero.Debug.TestData
{
  public class MyRedirectPageRenderer : RedirectPageRenderer
  {
    public MyRedirectPageRenderer() : base()
    {
      Field(x => x.Name).Text(opts => opts.MaxLength = 100);
      //Properties.FirstOrDefault(x => x.Method == "tab")
    }
  }
}
