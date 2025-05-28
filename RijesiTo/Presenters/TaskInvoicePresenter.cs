using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.ViewModels;
using TaskStatus = RijesiTo.Models.TaskStatus;

namespace RijesiTo.Presenters
{
    public class TaskInvoicePresenter
    {
        private readonly AppDbContext _context;
        private readonly ITaskInvoiceView _view;

        public TaskInvoicePresenter(AppDbContext context, ITaskInvoiceView view)
        {
            _context = context;
            _view = view;
        }

        public async Task LoadInvoiceDataAsync(int taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Status == TaskStatus.Completed);

            if (task == null) return;

            var invoice = new TaskInvoiceViewModel
            {
                TaskTitle = task.Title,
                Location = task.Location,
                DepositAmount = task.DepositAmount,
                TaskDate = task.DateTime,
                RequesterName = task.User.FirstName + " " + task.User.LastName,
                RequesterEmail = task.User.Email
            };

            _view.ShowInvoice(invoice);
        }
    }
}
