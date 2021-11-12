using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zero.Core.Entities;

namespace zero.Core.Blueprints
{
  /// <summary>
  /// 
  /// </summary>
  public class Blueprint<T> : Blueprint where T : ZeroEntity, new()
  {
    public List<BlueprintField<T>> Fields { get; private set; } = new();


    public Blueprint() : base(typeof(T))
    {
      Sync(x => x.Name);
      Sync(x => x.Alias);
      Sync(x => x.Sort);
      Sync(x => x.IsActive);
      Sync(x => x.LanguageId);
      Sync(x => x.Key);
    }


    /// <summary>
    /// Merges blueprint data into a model based on the configuration
    /// </summary>
    public virtual T Apply(T blueprint, T model)
    {
      // reset blueprint options for model in case no blueprint was found
      if (blueprint == null)
      {
        model.Blueprint = null;
        return model;
      }

      foreach (BlueprintField<T> field in Fields)
      {
        // do not sync disabled fields
        if (!field.IsSynced && !field.IsLocked)
        {
          continue;
        }

        // desynced fields are not synced with the blueprint
        // (but only if they are not locked)
        if (model.Blueprint.Desync.Contains(field.FieldName, StringComparer) && !field.IsLocked)
        {
          continue;
        }

        // apply new value to model property
        field.Apply(blueprint, model);
      }

      ApplyDefaults(blueprint, model);

      return model;
    }


    /// <inheritdoc />
    public override ZeroEntity Apply(ZeroEntity blueprint, ZeroEntity model) => Apply(blueprint as T, model as T);


    /// <summary>
    /// Updates meta data for an entity which should always be synchronized
    /// </summary>
    protected virtual void ApplyDefaults(T blueprint, T model)
    {
      model.Hash = blueprint.Hash;
      model.CreatedById = blueprint.CreatedById;
      model.CreatedDate = blueprint.CreatedDate;
      model.LastModifiedById = blueprint.LastModifiedById;
      model.LastModifiedDate = blueprint.LastModifiedDate;
    }


    /// <summary>
    /// Synchronize a field
    /// </summary>
    public BlueprintField<T> Sync(Expression<Func<T, object>> selector)
    {
      BlueprintField<T> field = Field(selector);
      field.IsSynced = true;
      return field;
    }


    /// <summary>
    /// Lock a field so it always get synchronized no and cannot be changed
    /// </summary>
    public BlueprintField<T> Lock(Expression<Func<T, object>> selector)
    {
      BlueprintField<T> field = Field(selector);
      field.IsLocked = true;
      return field;
    }


    /// <summary>
    /// Remove a field from synchronisation
    /// </summary>
    public void Remove(Expression<Func<T, object>> selector)
    {
      BlueprintField<T> field = new(selector);
      Fields.RemoveAll(x => x.FieldName.Equals(field.FieldName, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <summary>
    /// Get existing field or create a new one
    /// </summary>
    protected BlueprintField<T> Field(Expression<Func<T, object>> selector)
    {
      BlueprintField<T> tempField = new(selector);
      BlueprintField<T> storedField = Fields.FirstOrDefault(x => x.FieldName.Equals(tempField.FieldName, StringComparison.InvariantCultureIgnoreCase));

      if (storedField != null)
      {
        return storedField;
      }

      Fields.Add(tempField);
      return tempField;
    }
  }


  /// <summary>
  /// 
  /// </summary>
  public abstract class Blueprint
  {
    /// <summary>
    /// Type of the associated entity
    /// </summary>
    public Type ContentType { get; private set; }

    /// <summary>
    /// String comparer for property name comparison
    /// </summary>
    protected readonly StringComparer StringComparer = StringComparer.InvariantCultureIgnoreCase;


    public Blueprint(Type type)
    {
      ContentType = type;
    }

    /// <summary>
    /// Merges blueprint data into a model based on the configuration
    /// </summary>
    public abstract ZeroEntity Apply(ZeroEntity blueprint, ZeroEntity model);
  }


  public class DefaultBlueprint<T> : Blueprint<T> where T : ZeroEntity, new()
  {
    public DefaultBlueprint(Action<Blueprint<T>> expression)
    {
      expression(this);
    }
  }
}
