namespace Finch.Models;

public class FinchReference
{
  public FinchReference() { }

  public FinchReference(FinchEntity entity)
  {
    Id = entity.Id;
    Name = entity.Name;
  }

  public static FinchReference From(FinchEntity entity)
  {
    return entity == null ? null : new FinchReference(entity);
  }

  public static FinchReference From<T>(T entity, Func<T, string> transform) where T : FinchEntity
  {
    return entity == null ? null : new FinchReference()
    {
      Id = entity.Id,
      Name = transform(entity)
    };
  }

  public string Id { get; set; }

  public string Name { get; set; }
}
