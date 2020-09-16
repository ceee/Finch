using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Commerce.Entities;
using zero.Core.Validation;

namespace zero.TestData
{
  public class InvoiceDocument : DocumentTemplate
  {
    public string Text { get; set; }

    public InvoiceDocument()
    {
      Alias = "invoice";
      Name = "Invoice";
      Description = "Document containing invoice data";
      Filename = "invoice_{number}_{date}.pdf";
      TemplatePath = "~/Views/Pdf/Invoice.pdf";
      HasEditor = true;
      Validator = new ApplicationValidator();
    }

    public override Task<bool> IsAutoAttached(IOrder order, IMailTemplate mailTemplate, IAsyncDocumentSession session)
    {
      return Task.FromResult(mailTemplate.Alias == "order_confirmation");
    }

    public override Task AutoFill(IOrder order, IAsyncDocumentSession session)
    {
      Text = "Thanks you for your order " + order.Id;
      return Task.CompletedTask;
    }

    public override Task<bool> IsAvailable(IOrder order, IAsyncDocumentSession session)
    {
      return Task.FromResult(!order.IsRequest);
    }
  }
}
