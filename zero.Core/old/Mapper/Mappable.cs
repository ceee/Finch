//using zero.Core.Entities;

//namespace zero.Core.Mapper
//{
//  public class ZeroDtoEntity<T> : ZeroEntity where T : ZeroEntity, new()
//  {
//    public virtual T Map()
//    {
//      T result = new();
//      result.Id = Id;
//      result.Name = Name;
//      result.Alias = Alias;
//      result.Key = Key;
//      result.Sort = Sort;
//      result.IsActive = IsActive;
//      result.Hash = Hash;
//      result.LastModifiedById = LastModifiedById;
//      result.LastModifiedDate = LastModifiedDate;
//      result.CreatedById = CreatedById;
//      result.CreatedDate = CreatedDate;
//      result.Blueprint = Blueprint;
//      result.LanguageId = LanguageId;
//      result.Url = Url;

//      return result;
//    }


//    public virtual ZeroDtoEntity<T> Map(T model)
//    {
//      return new ZeroDtoEntity<T>();
//    }
//  }
//}
