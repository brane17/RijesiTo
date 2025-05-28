using Microsoft.AspNetCore.Mvc;
using RijesiTo.Data;
using RijesiTo.Extensions;
using RijesiTo.Interfaces;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;
using SelectPdf;

namespace RijesiTo.Controllers
{
    public class InvoiceController : Controller, ITaskInvoiceView
    {
        private readonly TaskInvoicePresenter _presenter;
        private TaskInvoiceViewModel _invoice;

        public InvoiceController(AppDbContext context)
        {
            _invoice = new TaskInvoiceViewModel();
            _presenter = new TaskInvoicePresenter(context, this);
        }

        public void ShowInvoice(TaskInvoiceViewModel invoice)
        {
            _invoice = invoice;
        }

        [HttpGet]
        public async Task<IActionResult> Download(int taskId)
        {
            await _presenter.LoadInvoiceDataAsync(taskId);

            string html = await this.RenderViewAsync("InvoiceTemplate", _invoice, true);

            var converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(html);

            byte[] pdf = doc.Save();
            doc.Close();

            return File(pdf, "application/pdf", $"Invoice_Task_{taskId}.pdf");
        }

    }
}
