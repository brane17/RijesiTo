using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.ViewModels;
using TaskStatus = RijesiTo.Models.TaskStatus;
namespace RijesiTo.Presenters
{
    public class WorkerPresenter
    {
        private readonly AppDbContext _context;
        private readonly IWorkerTaskView _view;

        public WorkerPresenter(AppDbContext context, IWorkerTaskView view)
        {
            _context = context;
            _view = view;
        }

        public async System.Threading.Tasks.Task LoadAvailableTasksAsync()
        {
            var tasks = await _context.Tasks
                .Where(t => t.Status == TaskStatus.NotStarted)
                .OrderByDescending(t => t.DateTime)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Location = t.Location,
                    DateTime = t.DateTime,
                    DepositAmount = t.DepositAmount,
                    Status = t.Status
                }).ToListAsync();

            _view.ShowTasks(tasks);
        }
    }
}
