using System.Threading.Tasks;
using zero.Communication;
using zero.Models;

namespace zero.Sqlite;

public interface IEntityModifiedHandler : IHandler
{
  Task Saved<T>(T model, bool update) where T : ZeroIdEntity, new() => update ? Updated(model) : Created(model);
  Task Created<T>(T model) where T : ZeroIdEntity, new();
  Task Updated<T>(T model) where T : ZeroIdEntity, new();
  Task Deleted<T>(T model) where T : ZeroIdEntity, new();
}


public class EmptyEntityModifiedHandler : IEntityModifiedHandler
{
  public Task Created<T>(T model) where T : ZeroIdEntity, new() => Task.CompletedTask;
  public Task Updated<T>(T model) where T : ZeroIdEntity, new() => Task.CompletedTask;
  public Task Deleted<T>(T model) where T : ZeroIdEntity, new() => Task.CompletedTask;
}