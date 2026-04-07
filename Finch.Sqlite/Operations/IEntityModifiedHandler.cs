using System.Threading.Tasks;
using Finch.Communication;
using Finch.Models;

namespace Finch.Sqlite;

public interface IEntityModifiedHandler : IHandler
{
  Task Saved<T>(T model, bool update) where T : FinchIdEntity, new() => update ? Updated(model) : Created(model);
  Task Created<T>(T model) where T : FinchIdEntity, new();
  Task Updated<T>(T model) where T : FinchIdEntity, new();
  Task Deleted<T>(T model) where T : FinchIdEntity, new();
}


public class EmptyEntityModifiedHandler : IEntityModifiedHandler
{
  public Task Created<T>(T model) where T : FinchIdEntity, new() => Task.CompletedTask;
  public Task Updated<T>(T model) where T : FinchIdEntity, new() => Task.CompletedTask;
  public Task Deleted<T>(T model) where T : FinchIdEntity, new() => Task.CompletedTask;
}