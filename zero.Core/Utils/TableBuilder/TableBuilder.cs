using System.Globalization;
using System.IO;
using System.Linq.Expressions;

namespace zero.Utils;

public class TableBuilder<T> : ITableBuilder<T>, IDisposable
{
  protected List<TableColumn<T>> Columns { get; set; }

  protected TableFormat Format { get; set; }

  protected CultureInfo Culture { get; set; }

  protected ILocalizer Localizer { get; set; }


  public TableBuilder(ILocalizer localizer, TableFormat format, CultureInfo culture = null)
  {
    Localizer = localizer;
    Format = format;

    Columns = new List<TableColumn<T>>();

    Culture = culture ?? new CultureInfo("de-DE"); // TODO get culture from zero config
    Culture.NumberFormat.CurrencyDecimalSeparator = ",";
    Culture.NumberFormat.CurrencyGroupSeparator = " "; 
  }


  /// <inheritdoc />
  public string GetExtension()
  {
    return Format switch
    {
      TableFormat.Csv => ".csv",
      TableFormat.Excel => ".xlsx",
      _ => throw new NotSupportedException($"The table format {Format} is not supported")
    };
  }


  /// <inheritdoc />
  public MemoryStream ToStream(IEnumerable<T> source)
  {
    ITableCreator<T> creator = Format switch
    {
      TableFormat.Csv => new CsvCreator<T>(Localizer, Columns),
      TableFormat.Excel => new ExcelCreator<T>(Localizer, Columns),
      _ => throw new NotSupportedException($"The table format {Format} is not supported")
    };

    return creator.CreateStream(source, Culture);
  }


  /// <inheritdoc />
  public async Task<MemoryStream> ToStream(IAsyncEnumerable<T> source)
  {
    ITableCreator<T> creator = Format switch
    {
      TableFormat.Csv => new CsvCreator<T>(Localizer, Columns),
      TableFormat.Excel => new ExcelCreator<T>(Localizer, Columns),
      _ => throw new NotSupportedException($"The table format {Format} is not supported")
    };

    return await creator.CreateStream(source, Culture);
  }


  /// <inheritdoc />
  public TableColumn<T> Column(string name, Expression<Func<T, object>> fieldSelector = null, double width = 15, TableColumnType type = TableColumnType.Default, int? position = null)
  {
    TableColumn<T> column = new()
    {
      Name = name,
      FieldSelector = fieldSelector?.Compile(),
      Width = width,
      ColumnType = type
    };

    if (position.HasValue && Columns.Count > position.Value)
    {
      Columns.Insert(position.Value, column);
    }
    else
    {
      Columns.Add(column);
    }

    return column;
  }


  /// <inheritdoc />
  public void Scope(Action<T> action)
  {
    action(default);
  }


  /// <inheritdoc />
  public TableColumn<T> Separator()
  {
    TableColumn<T> column = new()
    {
      IsSeparator = true,
      Width = 4
    };

    Columns.Add(column);

    return column;
  }


  /// <inheritdoc />
  public void Dispose()
  {
    //Stream?.Dispose();
    //Workbook?.Dispose();
  }
}


public interface ITableBuilder<T>
{
  /// <summary>
  /// Get file extension calculated by TableFormat
  /// </summary>
  string GetExtension();

  /// <summary>
  /// Add a new column to the table
  /// </summary>
  TableColumn<T> Column(string name, Expression<Func<T, object>> fieldSelector = null, double width = 12, TableColumnType type = TableColumnType.Default, int? position = null);

  /// <summary>
  /// 
  /// </summary>
  void Scope(Action<T> action);

  /// <summary>
  /// Adds an empty column which acts as a separator
  /// </summary>
  TableColumn<T> Separator();

  /// <summary>
  /// Creates a memory stream from the provided source
  /// </summary>
  MemoryStream ToStream(IEnumerable<T> source);

  /// <summary>
  /// Creates a memory stream from the provided source
  /// </summary>
  Task<MemoryStream> ToStream(IAsyncEnumerable<T> source);

  /// <summary>
  /// Disposes the underlying table creator
  /// </summary>
  void Dispose();
}
