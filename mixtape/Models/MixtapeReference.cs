namespace Mixtape.Models;

public class MixtapeReference
{
  public MixtapeReference() { }

  public MixtapeReference(MixtapeEntity entity)
  {
    Id = entity.Id;
    Name = entity.Name;
  }

  public static MixtapeReference From(MixtapeEntity entity)
  {
    return entity == null ? null : new MixtapeReference(entity);
  }

  public static MixtapeReference From<T>(T entity, Func<T, string> transform) where T : MixtapeEntity
  {
    return entity == null ? null : new MixtapeReference()
    {
      Id = entity.Id,
      Name = transform(entity)
    };
  }

  public string Id { get; set; }

  public string Name { get; set; }
}
