using System.Threading.Tasks;
using Mixtape.Communication;
using Mixtape.Models;

namespace Mixtape.Sqlite;

public interface IEntityModifiedHandler : IHandler
{
  Task Saved<T>(T model, bool update) where T : MixtapeIdEntity, new() => update ? Updated(model) : Created(model);
  Task Created<T>(T model) where T : MixtapeIdEntity, new();
  Task Updated<T>(T model) where T : MixtapeIdEntity, new();
  Task Deleted<T>(T model) where T : MixtapeIdEntity, new();
}


public class EmptyEntityModifiedHandler : IEntityModifiedHandler
{
  public Task Created<T>(T model) where T : MixtapeIdEntity, new() => Task.CompletedTask;
  public Task Updated<T>(T model) where T : MixtapeIdEntity, new() => Task.CompletedTask;
  public Task Deleted<T>(T model) where T : MixtapeIdEntity, new() => Task.CompletedTask;
}