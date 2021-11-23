namespace zero;

public class ZeroReference
{
  public ZeroReference() { }

  public ZeroReference(ZeroEntity entity)
  {
    Id = entity.Id;
    Name = entity.Name;
  }

  public static ZeroReference From(ZeroEntity entity)
  {
    return entity == null ? null : new ZeroReference(entity);
  }

  public static ZeroReference From<T>(T entity, Func<T, string> transform) where T : ZeroEntity
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
