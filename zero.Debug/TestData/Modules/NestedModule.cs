using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.TestData
{
  public class NestedModule : Module
  {
    public List<Module> Modules { get; set; } = new List<Module>();
  }
}
