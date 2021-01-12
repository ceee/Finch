using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace zero.Core.Utils
{
  public class ExcelCreator<T> : ITableCreator<T>
  {
    private MemoryStream Stream { get; set; }

    private XLWorkbook Workbook { get; set; }

    private IEnumerable<TableColumn<T>> Columns { get; set; }

    private const string DATE_FORMAT = "dd. MM. yyyy";

    private const string DATETIME_FORMAT = "dd. MM. yyyy, HH:ii";

    private const string BOOL_YES = "Ja"; // TODO translate

    private const string BOOL_NO = "Nein";

    private const string LINK_NAME = "Link";

    public const string CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private CultureInfo Culture = null;


    public ExcelCreator(IEnumerable<TableColumn<T>> columns)
    {
      Columns = columns;
    }


    /// <summary>
    /// Create a readable XLSX stream from a source enumerable
    /// </summary>
    public MemoryStream CreateStream(IEnumerable<T> source, CultureInfo culture)
    {
      Culture = culture;
      Workbook = Create(true, out int lineNumber);
      foreach (T item in source)
      {
        AddRow(Workbook.Worksheets.First(), lineNumber++, Columns, item);
      }
      Stream = new MemoryStream();
      Workbook.SaveAs(Stream);
      Stream.Position = 0;
      return Stream;
    }


    /// <summary>
    /// Create a readable XLSX stream from a source enumerable
    /// </summary>
    public async Task<MemoryStream> CreateStream(IAsyncEnumerable<T> source, CultureInfo culture, CancellationToken token = default)
    {
      Culture = culture;
      Workbook = Create(true, out int lineNumber);
      await foreach (T item in source.WithCancellation(token))
      {
        AddRow(Workbook.Worksheets.First(), lineNumber++, Columns, item);
      }
      Stream = new MemoryStream();
      Workbook.SaveAs(Stream);
      Stream.Position = 0;
      return Stream;
    }


    /// <summary>
    /// Create a XLSX from a source enumerable
    /// </summary>
    protected XLWorkbook Create(IEnumerable<T> source, bool headerRow = true)
    {
      XLWorkbook workbook = new XLWorkbook();
      IXLWorksheet worksheet = workbook.Worksheets.Add("Data");

      int lineNumber = 1;


      // add metadata + defaults
      workbook.Author = "Laola";
      workbook.RowHeight = 20;

      // add header
      if (headerRow)
      {
        AddRow(worksheet, lineNumber++, Columns, default, true);
        worksheet.Row(1).Height = 30;
      }

      // set column widths
      for (int i = 0; i < Columns.Count(); i++)
      {
        TableColumn<T> column = Columns.ElementAt(i);
        worksheet.Column(i + 1).Width = column.Width;
      }

      // add rows
      foreach (T item in source)
      {
        AddRow(worksheet, lineNumber++, Columns, item);
      }

      return workbook;
    }


    /// <summary>
    /// Create a XLSX from a source enumerable
    /// </summary>
    protected XLWorkbook Create(bool headerRow, out int lineNumber)
    {
      XLWorkbook workbook = new XLWorkbook();
      IXLWorksheet worksheet = workbook.Worksheets.Add("Data");

      lineNumber = 1;

      // add metadata + defaults
      workbook.Author = "Laola";
      workbook.RowHeight = 20;

      // add header
      if (headerRow)
      {
        AddRow(worksheet, lineNumber++, Columns, default, true);
        worksheet.Row(1).Height = 30;
      }

      // set column widths
      for (int i = 0; i < Columns.Count(); i++)
      {
        TableColumn<T> column = Columns.ElementAt(i);
        worksheet.Column(i + 1).Width = column.Width;
      }

      return workbook;
    }


    /// <summary>
    /// Appends a line to the XLSX
    /// </summary>
    protected void AddRow(IXLWorksheet builder, int lineNumber, IEnumerable<TableColumn<T>> columns, T item, bool isHeader = false)
    {
      int index = 0;

      foreach (TableColumn<T> column in columns)
      {
        object value = isHeader ? column.Name : (column.FieldSelector != null && column.CanRender(item) ? column.FieldSelector(item) : String.Empty);
        object property = MapProperty(value, column, out XLDataType dataType);

        IXLCell cell = builder.Cell(lineNumber, index + 1)
          .SetValue(property)
          .SetDataType(dataType);

        if (isHeader)
        {
          cell.Style.Font.SetBold();
          cell.Style.Fill.BackgroundColor = XLColor.WhiteSmoke;
        }

        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

        if (!isHeader && column.ColumnType == TableColumnType.Link)
        {
          cell.SetValue(LINK_NAME);
          cell.Hyperlink = new XLHyperlink(value.ToString());
        }
        if (!isHeader && column.ColumnType == TableColumnType.Currency)
        {
          cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        }

        index += 1;
      }

      if (!isHeader)
      {
        builder.Row(lineNumber).Height = 20;
      }
    }


    /// <summary>
    /// Map a property based on its type
    /// </summary>
    protected object MapProperty(object value, TableColumn<T> column, out XLDataType dataType)
    {
      dataType = XLDataType.Text;

      if (value == null)
      {
        return String.Empty;
      }

      if (value is bool)
      {
        return (bool)value ? BOOL_YES : BOOL_NO;
      }
      else if (value is decimal || value is float || value is double)
      {
        dataType = XLDataType.Number;
        decimal result = Decimal.Round(Convert.ToDecimal(value), 2);

        if (column.ColumnType == TableColumnType.Currency)
        {
          dataType = XLDataType.Text;
          return result.ToString("C2", Culture);
        }

        return result;
      }
      else if (value is int)
      {
        dataType = XLDataType.Number;
        return value;
      }
      else if (value is DateTime)
      {
        return ((DateTime)value).ToString(DATE_FORMAT);
      }
      else if (value is DateTime?)
      {
        var val = (DateTime?)value;
        return val.HasValue ? val.Value.ToString(DATE_FORMAT) : String.Empty;
      }
      else if (value is DateTimeOffset)
      {
        return ((DateTimeOffset)value).ToString(DATE_FORMAT);
      }
      else if (value is DateTimeOffset?)
      {
        var val = (DateTimeOffset?)value;
        return val.HasValue ? val.Value.ToString(DATE_FORMAT) : String.Empty;
      }

      return value;
    }


    public void Dispose()
    {
      Stream?.Dispose();
      Workbook?.Dispose();
    }
  }
}
