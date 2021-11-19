using FluentValidation;
using zero.Core.Collections;

namespace zero.Core.Routing
{
  public class RouteRedirectCollection : CollectionBase<RouteRedirect>, IRouteRedirectCollection
  {
    public RouteRedirectCollection(ICollectionContext collectionContext, IValidator<RouteRedirect> validator = null) : base(collectionContext, validator)
    {
      
    }
  }


  public interface IRouteRedirectCollection : ICollectionBase<RouteRedirect>
  {
    
  }
}
