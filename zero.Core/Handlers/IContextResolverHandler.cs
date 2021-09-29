using System.Threading.Tasks;

namespace zero.Core.Handlers
{
  public interface IContextResolverHandler : IHandler
  {
    Task AfterResolve(IZeroContext context);
  }
}
