using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace zero;

public class CsvCreator<T> : ITableCreator<T>
{
  private MemoryStream Stream { get; set; }

  private StreamWriter StreamWriter { get; set; }

  private IEnumerable<TableColumn<T>> Columns { get; set; }

  private const char CHAR_DBL_QUOTE = '"';

  private const char CHAR_QUOTE = '\'';

  private const char CHAR_COLON = ',';

  private const char CHAR_SEMICOLON = ';';

  private const string DATE_FORMAT = "dd. MM. yyyy"; // TODO date format from culture

  private const string BOOL_YES = "Ja"; // TODO translation

  private const string BOOL_NO = "Nein";


  public CsvCreator(IEnumerable<TableColumn<T>> columns)
  {
    Columns = columns;
  }


  /// <summary>
  /// Create a readable stream from a source enumerable
  /// </summary>
  public MemoryStream CreateStream(IEnumerable<T> source, CultureInfo culture)
  {
    StringBuilder builder = Create(true);
    foreach (T item in source)
    {
      List<object> values = Columns.Select(c => c.FieldSelector != null && c.CanRender(item) ? c.FieldSelector(item) : String.Empty).ToList();
      AppendLine(builder, values);
    }
    Stream = new MemoryStream();
    StreamWriter = new StreamWriter(Stream, Encoding.UTF8);

    StreamWriter.Write(builder);
    StreamWriter.Flush();
    Stream.Position = 0;

    return Stream;
  }


  /// <summary>
  /// Create a readable stream from a source enumerable
  /// </summary>
  public async Task<MemoryStream> CreateStream(IAsyncEnumerable<T> source, CultureInfo culture, CancellationToken token = default)
  {
    StringBuilder builder = Create(true);
    await foreach (T item in source.WithCancellation(token))
    {
      List<object> values = Columns.Select(c => c.FieldSelector != null && c.CanRender(item) ? c.FieldSelector(item) : String.Empty).ToList();
      AppendLine(builder, values);
    }
    Stream = new MemoryStream();
    StreamWriter = new StreamWriter(Stream, Encoding.UTF8);

    StreamWriter.Write(builder);
    StreamWriter.Flush();
    Stream.Position = 0;

    return Stream;
  }


  /// <summary>
  /// Create a CSV from a source enumerable
  /// </summary>
  protected StringBuilder Create(bool headerRow = true)
  {
    StringBuilder csv = new StringBuilder();

    // add header
    if (headerRow)
    {
      AppendLine(csv, Columns.Select(c => c.Name));
    }

    return csv;
  }


  /// <summary>
  /// Appends a line to the CSV
  /// </summary>
  protected void AppendLine(StringBuilder builder, IEnumerable<object> values)
  {
    int index = 0;
    int length = values.Count();

    foreach (object value in values)
    {
      string property = MapProperty(value);

      if (!String.IsNullOrWhiteSpace(property))
      {
        builder.Append(CHAR_DBL_QUOTE + property + CHAR_DBL_QUOTE);
      }
      if (++index < length)
      {
        builder.Append(CHAR_COLON);
      }
    }

    builder.AppendLine();
  }


  /// <summary>
  /// Map a property to a string based on its type
  /// </summary>
  protected string MapProperty(object value)
  {
    string property = String.Empty;

    if (value == null)
    {
      return property;
    }

    if (value is string)
    {
      property = (string)value;
    }
    else if (value is bool)
    {
      property = (bool)value ? BOOL_YES : BOOL_NO;
    }
    else if (value is decimal || value is double || value is float)
    {
      property = value.ToString().Replace(",", ".");
    }
    else if (value is DateTime)
    {
      property = ((DateTime)value).ToString(DATE_FORMAT);
    }
    else if (value is DateTime?)
    {
      var val = (DateTime?)value;
      property = val.HasValue ? val.Value.ToString(DATE_FORMAT) : String.Empty;
    }
    else if (value is DateTimeOffset)
    {
      property = ((DateTimeOffset)value).ToString(DATE_FORMAT);
    }
    else if (value is DateTimeOffset?)
    {
      var val = (DateTimeOffset?)value;
      property = val.HasValue ? val.Value.ToString(DATE_FORMAT) : String.Empty;
    }
    else
    {
      property = value.ToString();
    }

    return property.Replace(CHAR_DBL_QUOTE, CHAR_QUOTE).Replace(CHAR_COLON, CHAR_SEMICOLON);
  }


  public void Dispose()
  {
    Stream?.Dispose();
    StreamWriter?.Dispose();
  }
}
