using System.Collections.Generic;
using System.Linq;
using zero.Commerce.Entities;
using zero.Core.Api;

namespace zero.Debug.Sync
{
  public class PropertyBlueprintHandler : BlueprintHandler<IProperty>
  {
    protected override void OnBlueprintCreate(IProperty blueprint, IProperty model)
    {
      model.Description = blueprint.Description;
      model.Limit = blueprint.Limit;
      model.ForFilter = blueprint.ForFilter;
      model.AllowText = blueprint.AllowText;
      model.IconSet = blueprint.IconSet;
      model.Type = blueprint.Type;
      model.SortingMethod = blueprint.SortingMethod;

      List<IPropertyValue> values = blueprint.Values.ToList();
      string[] valueIds = values.Select(x => x.Id).ToArray();

      foreach (IPropertyValue value in values)
      {
        string desyncKey = $"Values[{value.Id}].";
        string[] desynced = model.Blueprint.Desync.Where(x => x.StartsWith(desyncKey)).ToArray();
        IPropertyValue match = model.Values.FirstOrDefault(x => x.Id == value.Id);

        if (match != null)
        {
          if (desynced.Contains(desyncKey + nameof(value.Value)))
          {
            value.Value = match.Value;
            value.Alias = Safenames.Alias(value.Value);
          }
          if (value is ColorPropertyValue && desynced.Contains(desyncKey + "Hex"))
          {
            ((ColorPropertyValue)value).Hex = ((ColorPropertyValue)match).Hex;
          }
        }
      }

      foreach (IPropertyValue value in model.Values.Where(x => !valueIds.Contains(x.Id)))
      {
        values.Add(value);
      }

      model.Values = values;
    }
  }
}
