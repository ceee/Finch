namespace zero.Backoffice.Modules.Countries;

public class CountryPickerProvider : PickerProvider<Country>
{
  public CountryPickerProvider(IStoreOperations ops) : base(ops) { }


  /// <inheritdoc />
  protected override PickerPreviewModel ConvertToPreview(Country source)
  {
    return new()
    {
      Id = source.Id,
      Name = source.Name,
      Icon = "flag-" + source.Code.ToLowerInvariant()
    };
  }
}