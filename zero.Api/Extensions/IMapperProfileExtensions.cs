namespace zero.Api.Extensions;

public static class IMapperProfileExtensions
{
  /// <summary>
  /// Map data for a zero entity BasicModel
  /// </summary>
  public static void MapBasicData<TSource, TDestination>(this IMapperProfile profile, TSource source, TDestination target)
    where TSource : ZeroEntity
    where TDestination : BasicModel<TSource>
  {
    target.Id = source.Id;
    target.Name = source.Name;
    target.Alias = source.Alias;
    target.Key = source.Key;
    target.IsActive = source.IsActive;
    target.CreatedDate = source.CreatedDate;
  }


  /// <summary>
  /// Map data for a zero entity DiplayModel
  /// </summary>
  public static void MapDisplayData<TSource, TDestination>(this IMapperProfile profile, TSource source, TDestination target)
    where TSource : ZeroEntity
    where TDestination : DisplayModel<TSource>
  {
    target.Id = source.Id;
    target.Name = source.Name;
    target.Alias = source.Alias;
    target.Key = source.Key;
    target.IsActive = source.IsActive;
    target.CreatedDate = source.CreatedDate;

    target.Sort = source.Sort;
    target.Hash = source.Hash;
    target.LastModifiedById = source.LastModifiedById;
    target.LastModifiedDate = source.LastModifiedDate;
    target.CreatedById = source.CreatedById;
    target.LanguageId = source.LanguageId;
  }


  /// <summary>
  /// Map data for a zero entity SaveModel
  /// </summary>
  public static void MapSaveData<TSource, TDestination>(this IMapperProfile profile, TSource source, TDestination target)
    where TSource : SaveModel<TDestination>
    where TDestination : ZeroEntity
  {
    target.Name = source.Name;
    target.Alias = source.Alias;
    target.Key = source.Key;
    target.Sort = source.Sort;
    target.IsActive = source.IsActive;
  }
}
