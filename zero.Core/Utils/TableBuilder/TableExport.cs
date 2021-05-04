using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Utils
{
  public abstract class TableExport<TEntity> : ITableExport<TEntity> where TEntity : ZeroEntity
  {
    public virtual async Task<Stream> Export(TableFormat format = TableFormat.Excel)
    {
      ITableBuilder<TEntity> builder = new TableBuilder<TEntity>(format);

      Build(builder);

      IAsyncEnumerable<TEntity> items = Load();

      MemoryStream stream = await builder.ToStream(items);

      return stream;
    }


    protected virtual async IAsyncEnumerable<TEntity> Load()
    {
      await Task.Delay(0);
      yield break;
    }


    protected virtual void Build(ITableBuilder<TEntity> builder)
    {
      builder.Column("Id").For(c => c.Id).Size(40);
      builder.Column("Name").For(c => c.Name).Size(40);
    }
  }


  public interface ITableExport<TEntity> where TEntity : ZeroEntity
  {
    Task<Stream> Export(TableFormat format = TableFormat.Excel);
  }
}
