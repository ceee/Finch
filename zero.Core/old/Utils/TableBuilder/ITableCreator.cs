using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace zero.Core.Utils
{
  public interface ITableCreator<T> : IDisposable
  {
    /// <summary>
    /// Create a readable stream from a source enumerable
    /// </summary>
    Task<MemoryStream> CreateStream(IAsyncEnumerable<T> source, CultureInfo culture, CancellationToken token = default);

    /// <summary>
    /// Create a readable stream from a source enumerable
    /// </summary>
    MemoryStream CreateStream(IEnumerable<T> source, CultureInfo culture);
  }
}
