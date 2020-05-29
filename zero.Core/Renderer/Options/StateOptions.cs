using System.Collections.Generic;

namespace zero.Core.Renderer
{
  public class StateOptions : AbstractFieldInputOptions
  {
    public class Item
    {
      public string Label { get; set; }

      public object Value { get; set; }
    }

    public List<Item> Items { get; set; } = new List<Item>();

    public void Add(string label, object value)
    {
      Items.Add(new Item()
      {
        Label = label,
        Value = value
      });
    }
  }
}
