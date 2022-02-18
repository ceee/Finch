using System.Linq.Expressions;

namespace zero.Utils;

public class TableColumn<T>
{
  public string Name { get; set; }

  public bool IsSeparator { get; set; }

  public Func<T, object> FieldSelector { get; set; }

  public Func<T, bool> CanRender { get; set; } = x => true;

  public double Width { get; set; }

  public bool IsBold { get; set; }

  public TableColumnType ColumnType { get; set; }

  public TableColumn<T> Type(TableColumnType type)
  {
    ColumnType = type;
    return this;
  }

  public TableColumn<T> Size(double width)
  {
    Width = width;
    return this;
  }

  public TableColumn<T> For(Expression<Func<T, object>> fieldSelector, Expression<Func<T, bool>> canRender = null)
  {
    FieldSelector = fieldSelector.Compile();
    CanRender = canRender != null ? canRender.Compile() : CanRender;
    return this;
  }

  public TableColumn<T> Currency()
  {
    ColumnType = TableColumnType.Currency;
    return this;
  }

  public TableColumn<T> Link()
  {
    ColumnType = TableColumnType.Link;
    return this;
  }

  public TableColumn<T> Bold()
  {
    IsBold = true;
    return this;
  }
}


public enum TableColumnType
{
  Default,
  Currency,
  Link
}
