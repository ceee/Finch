namespace zero.Backoffice.Abstractions;

public class PickerProvider<T> : IPickerProvider<T> where T : ZeroIdEntity, new()
{
  protected IStoreOperations Operations { get; set; }


  public PickerProvider(IStoreOperations operations)
  {
    Operations = operations;
  }


  /// <inheritdoc />
  public virtual async Task<List<PickerPreviewModel>> GetPreviews(IEnumerable<string> ids)
  {
    Dictionary<string, T> result = await Operations.Load<T>(ids);
    return result.Select(x =>
    {
      if (x.Value == null)
      {
        return GenerateNotFoundPreview(x.Key);
      }
      return ConvertToPreview(x.Value);
    }).ToList();
  }


  /// <inheritdoc />
  public virtual async Task<Paged<PickerModel>> PickFrom(int pageNumber, int pageSize = 50, ListQuery<T> query = default)
  {
    Paged<T> result = await Operations.Load<T>(pageNumber, pageSize, q => q.Filter(query));
    return result.MapTo(ConvertToModel);
  }


  /// <summary>
  /// Converts a source item to picker model which is output in the list picker
  /// </summary>
  protected virtual PickerModel ConvertToModel(T source)
  {
    PickerModel model = new() { Id = source.Id };

    if (source is ZeroEntity)
    {
      model.Name = (source as ZeroEntity).Name;
      model.IsActive = (source as ZeroEntity).IsActive;
    }

    return model;
  }


  /// <summary>
  /// Converts a source item to picker model which is output in the list picker.
  /// Renders an error when the source is null.
  /// </summary>
  protected virtual PickerPreviewModel ConvertToPreview(T source)
  {
    PickerPreviewModel model = new()
    {
      Id = source.Id,
      Icon = "fth-box"
    };

    if (source is ZeroEntity)
    {
      model.Name = (source as ZeroEntity).Name;
    }
    else
    {
      model.Name = "[object #" + source.Id + "]";
    }

    return model;
  }


  /// <summary>
  /// Generates a preview which displays a not-found error
  /// </summary>
  protected virtual PickerPreviewModel GenerateNotFoundPreview(string id)
  {
    return new()
    {
      HasError = true,
      Icon = "fth-alert-circle color-red",
      Id = id,
      Name = "@errors.preview.notfound",
      Text = "@errors.preview.notfound_text"
    };
  }
}


public interface IPickerProvider<T> where T : ZeroIdEntity, new()
{
  /// <summary>
  /// Get previews which are displayed in the editor field
  /// </summary>
  Task<List<PickerPreviewModel>> GetPreviews(IEnumerable<string> ids);

  /// <summary>
  /// Items to choose from when the picker is open.
  /// The picker supports paging as well as filters.
  /// </summary>
  Task<Paged<PickerModel>> PickFrom(int pageNumber, int pageSize = 50, ListQuery<T> query = null);
}