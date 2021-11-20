using System.IO;

namespace zero.Utils;

public abstract class TableExport<TEntity> : ITableExport<TEntity> where TEntity : ZeroIdEntity
{
  public virtual async Task<Stream> Export(TableFormat format = TableFormat.Excel)
  {
    ITableBuilder<TEntity> builder = new TableBuilder<TEntity>(format);

    await Warmup();

    Build(builder);

    IAsyncEnumerable<TEntity> items = Load();

    MemoryStream stream = await builder.ToStream(items);

    return stream;
  }


  protected virtual Task Warmup() => Task.CompletedTask;


  protected virtual async IAsyncEnumerable<TEntity> Load()
  {
    await Task.Delay(0);
    yield break;
  }


  protected virtual void Build(ITableBuilder<TEntity> builder)
  {
    builder.Column("Id").For(c => c.Id).Size(40);
  }
}


public interface ITableExport<TEntity> where TEntity : ZeroIdEntity
{
  Task<Stream> Export(TableFormat format = TableFormat.Excel);
}