using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections.Tests
{
  public class CountryCollection : EntityCollection<Country>
  {
    /// <inheritdoc />
    public override async Task<ValidationResult> Validate(ICollectionContext ctx, Country model)
    {
      return await new CountryValidator(ctx.Store).ValidateAsync(model);
    }
  }


  public class CountryDto : ZeroEntity
  {
    public bool IsPreferred { get; set; }
    public string Code { get; set; }
  }


  public class EntityCollection<TEntity, TDto> where TEntity : ZeroEntity where TDto : ZeroEntity
  {
    /// <inheritdoc />
    public virtual Task<ValidationResult> Validate(ICollectionContext ctx, TDto model)
    {
      return Task.FromResult(new ValidationResult());
    }


    public virtual Task<EntityResult<TEntity>> Save(ICollectionContext ctx, TDto model)
    {
      throw new NotImplementedException();
    }
  }


  public class EntityCollection<T> : EntityCollection<T, T> where T : ZeroEntity { }
}
