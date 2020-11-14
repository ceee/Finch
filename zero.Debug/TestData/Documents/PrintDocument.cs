using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Commerce.Entities;
using zero.Core.Entities;

namespace zero.TestData
{
  public class PrintDocument : DocumentTemplate
  {
    public PrintDocument()
    {
      Alias = "print";
      Name = "Print-Job";
      Description = "Information for the print service";
      Filename = "print_{number}_{date}.pdf";
      TemplatePath = "~/Views/Pdf/Print.pdf";
    }


    public override Task<bool> IsAutoAttached(IOrder order, IMailTemplate mailTemplate, IAsyncDocumentSession session)
    {
      return Task.FromResult(mailTemplate.Alias == "print_task");
    }

    public override Task<bool> IsAvailable(IOrder order, IAsyncDocumentSession session)
    {
      return Task.FromResult(!order.IsRequest);
    }
  }
}
