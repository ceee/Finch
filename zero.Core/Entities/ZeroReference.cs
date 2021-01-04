using System;

namespace zero.Core.Entities
{
  public class ZeroReference
  {
    public ZeroReference() { }

    public ZeroReference(IZeroEntity entity)
    {
      Id = entity.Id;
      Name = entity.Name;
    }

    public static ZeroReference From(IZeroEntity entity)
    {
      return entity == null ? null : new ZeroReference(entity);
    }

    public static ZeroReference From<T>(T entity, Func<T, string> transform) where T : IZeroEntity
    {
      return entity == null ? null : new ZeroReference()
      {
        Id = entity.Id,
        Name = transform(entity)
      };
    }

    public string Id { get; set; }

    public string Name { get; set; }
  }
}
