using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace zero.Core.Renderer
{
  public class RendererFinder
  {
    static RendererFinder()
    {

    }


    public static IRenderer<T> FindBy<T>()
    {
      return null;
    }


    public static AbstractGenericRenderer FindBy(Type type)
    {
      //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

      //List<Type> types = assemblies
      //  .Where(assembly => !assembly.IsDynamic)
      //  .SelectMany(assembly => assembly.GetExportedTypes())
      //  .ToList();

      List<Type> types = Assembly.GetExecutingAssembly().GetExportedTypes().ToList();

      return null;
    }
  }
}
