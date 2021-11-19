using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace zero;

/// <summary>
/// 
/// </summary>
public class Blueprint<T> : Blueprint where T : ZeroEntity, new()
{
  public List<BlueprintField<T>> UnlockedFields { get; private set; } = new();

  /// <summary>
  /// These fields are not copied by the ObjectCloner 
  /// as they are either locked or copied manually by ApplyDefaults()
  /// </summary>
  protected static string[] DefaultUncopyFields { get; set; } = new[] { "id", "url", "name", "alias", "key", "sort", "isActive", "hash", "languageId", "createdById", "createdDate", "lastModifiedById", "lastModifiedDate", "blueprint" };


  public Blueprint() : base(typeof(T)) { }


  /// <summary>
  /// Merges blueprint data into a model based on the configuration
  /// </summary>
  public virtual T Apply(T blueprint, T model)
  {
    // reset blueprint options for model in case no blueprint was found
    if (blueprint == null || model.Blueprint == null)
    {
      model.Blueprint = null;
      return model;
    }

    // copy all properties which are synced from the blueprint to the model
    ApplyDefaults(blueprint, model);
    ObjectCopycat.CopyProperties(blueprint, model, DefaultUncopyFields.Union(model.Blueprint.Desync).ToArray());

    return model;
  }


  /// <inheritdoc />
  public override ZeroEntity Apply(ZeroEntity blueprint, ZeroEntity model) => Apply(blueprint as T, model as T);


  /// <summary>
  /// Update default entity data
  /// </summary>
  protected virtual void ApplyDefaults(T blueprint, T model)
  {
    if (!model.Blueprint.Desync.Contains(nameof(ZeroEntity.Name), StringComparer))
    {
      model.Name = blueprint.Name;
      model.Alias = blueprint.Alias;
    }
    if (!model.Blueprint.Desync.Contains(nameof(ZeroEntity.Key), StringComparer))
    {
      model.Key = blueprint.Key;
    }
    if (!model.Blueprint.Desync.Contains(nameof(ZeroEntity.Sort), StringComparer))
    {
      model.Sort = blueprint.Sort;
    }
    if (!model.Blueprint.Desync.Contains(nameof(ZeroEntity.IsActive), StringComparer))
    {
      model.IsActive = blueprint.IsActive;
    }

    // these values can't be overridden
    model.Hash = blueprint.Hash;
    model.LanguageId = blueprint.LanguageId;
    model.CreatedById = blueprint.CreatedById;
    model.CreatedDate = blueprint.CreatedDate;
    model.LastModifiedById = blueprint.LastModifiedById;
    model.LastModifiedDate = blueprint.LastModifiedDate;
  }


  /// <summary>
  /// Lock a field so it always get synchronized no and cannot be changed
  /// </summary>
  public void LockAll()
  {
    UnlockedFields = new();
  }


  /// <summary>
  /// Lock a field so it always get synchronized no and cannot be changed
  /// </summary>
  public void Lock(Expression<Func<T, object>> selector)
  {
    RemoveField(selector);
  }


  public void Unlock(Expression<Func<T, object>> selector)
  {
    AddField(selector);
  }


  public void UnlockDefaults()
  {
    AddField(x => x.Name);
    AddField(x => x.Alias);
    AddField(x => x.Sort);
    AddField(x => x.Key);
    AddField(x => x.IsActive);
  }


  /// <inheritdoc />
  public override IEnumerable<string> GetUnlockedFieldNames()
  {
    foreach (BlueprintField<T> field in UnlockedFields)
    {
      yield return field.FieldName.ToCamelCaseId();
    }
  }


  /// <summary>
  /// Get existing field or create a new one
  /// </summary>
  protected BlueprintField<T> AddField(Expression<Func<T, object>> selector)
  {
    BlueprintField<T> tempField = new(selector);
    BlueprintField<T> storedField = UnlockedFields.FirstOrDefault(x => x.FieldName.Equals(tempField.FieldName, StringComparison.InvariantCultureIgnoreCase));

    if (storedField != null)
    {
      return storedField;
    }

    UnlockedFields.Add(tempField);
    return tempField;
  }


  /// <summary>
  /// Removes a field
  /// </summary>
  protected void RemoveField(Expression<Func<T, object>> selector)
  {
    BlueprintField<T> tempField = new(selector);

    if (tempField != null)
    {
      UnlockedFields.RemoveAll(x => x.FieldName.Equals(tempField.FieldName, StringComparison.InvariantCultureIgnoreCase));
    }
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

  public string Alias { get; private set; }

  /// <summary>
  /// String comparer for property name comparison
  /// </summary>
  protected readonly StringComparer StringComparer = StringComparer.InvariantCultureIgnoreCase;


  public Blueprint(Type type)
  {
    ContentType = type;
    Alias = type.Name.ToCamelCaseId();
  }

  /// <summary>
  /// Merges blueprint data into a model based on the configuration
  /// </summary>
  public abstract ZeroEntity Apply(ZeroEntity blueprint, ZeroEntity model);

  /// <summary>
  /// Get all fields which have been unlocked
  /// </summary>
  public abstract IEnumerable<string> GetUnlockedFieldNames();
}


public class DefaultBlueprint<T> : Blueprint<T> where T : ZeroEntity, new()
{
  public DefaultBlueprint(Action<Blueprint<T>> expression = null) : base()
  {
    expression?.Invoke(this);
  }
}
