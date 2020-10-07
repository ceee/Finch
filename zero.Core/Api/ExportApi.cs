//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Session;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using zero.Core.Entities;
//using zero.Core.Extensions;

//namespace zero.Core.Api
//{
//  public class ExportApi : AppAwareBackofficeApi, IExportApi
//  {
//    public ExportApi(IBackofficeStore store) : base(store)
//    {
//    }


//    /// <inheritdoc />
//    public Space Export<T>(T config) where T : IExportConfig
//    {
//      Type type = typeof(T);
//      return GetAll().FirstOrDefault(x => x.Type == type);
//    }
//  }


//  public interface IExportApi
//  {
//    /// <summary>
//    /// Returns a space by a defined generic
//    /// </summary>
//    Space GetBy<T>() where T : ISpaceContent;
//  }
//}
