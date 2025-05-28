namespace RijesiTo.ViewModels
{
    public class TaskInvoiceViewModel
    {
        public string TaskTitle { get; set; }
        public string Location { get; set; }
        public double DepositAmount { get; set; }
        public DateTime TaskDate { get; set; }

        public string RequesterName { get; set; }
        public string RequesterEmail { get; set; }
    }
}
