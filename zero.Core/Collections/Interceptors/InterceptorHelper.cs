using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public class InterceptorHelper
  {
    protected ICollectionInterceptorHandler Handler { get; set; }


    public InterceptorHelper(ICollectionInterceptorHandler handler)
    {
      Handler = handler;
    }


    public async Task<InterceptorInstruction<T, CollectionInterceptor<T>.UpdateParameters>> Updating<T>(T model) where T : ZeroEntity
    {
      CollectionInterceptor<T>.UpdateParameters parameters = new();
      InterceptorInstruction<T, CollectionInterceptor<T>.UpdateParameters> instruction = Handler.Create<T, CollectionInterceptor<T>.UpdateParameters>("update", parameters) ?? new();

      await instruction.HandleBefore(x => x.Updating(instruction.Parameters));

      return instruction;
    }
  }
}
