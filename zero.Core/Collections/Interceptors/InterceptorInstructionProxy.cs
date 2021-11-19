namespace zero.Core;
using System.Threading.Tasks;
using zero.Core.Entities;

public class InterceptorRunnerProxy
{
  public InterceptorInstructionProxy CreateInstruction<T>(InterceptorType type, T model) where T : ZeroIdEntity
  {
    return null;
    //return new InterceptorInstructionProxy(Context, collection, type, model);
  }
}


public class InterceptorInstructionProxy
{
  public async Task<EntityResult> Run()
  {
    await Task.Delay(0);
    return new EntityResult();
  }


  public async Task Complete()
  {
    await Task.Delay(0);
  }
}