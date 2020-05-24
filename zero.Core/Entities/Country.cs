namespace zero.Core.Entities
{
  public class Country : ZeroEntity, ICountry
  {
    /// <inheritdoc />
    public bool IsPreferred { get; set; }

    /// <inheritdoc />
    public string Code { get; set; }

    /// <inheritdoc />
    public string LanguageId { get; set; }
  }


  public interface ICountry : IZeroEntity, ILanguageAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Preferred countries are displayed on top in lists
    /// </summary>
    bool IsPreferred { get; set; }

    /// <summary>
    /// Country code (ISO 3166-1)
    /// </summary>
    string Code { get; set; }
  }
}
