using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface ILinkProvider
  {
    string Name { get; }

    string Alias { get; }

    /// <summary>
    /// 
    /// </summary>
    Task<string> ResolveLink(ILink link);
  }
}
