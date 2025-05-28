using RijesiTo.Interfaces;
using RijesiTo.ViewModels;

namespace RijesiTo.Presenters
{
    public class NullInvoiceView : ITaskInvoiceView
    {
        public void ShowInvoice(TaskInvoiceViewModel invoice)
        {
            throw new NotImplementedException();
        }
    }
}
