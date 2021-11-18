namespace zero.Core;
using zero.Core.Entities;

public class InterceptorRunner<T> where T : ZeroIdEntity
{
  public InterceptorRunner()
  {

  }


  public InterceptorInstruction<T> CreateInstruction(InterceptorType type, T model)
  {
    return new InterceptorInstruction<T>(type, model);
  }
}