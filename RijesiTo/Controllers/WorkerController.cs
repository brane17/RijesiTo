using Microsoft.AspNetCore.Mvc;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;

namespace RijesiTo.Controllers
{
    public class WorkerController : Controller, IWorkerTaskView
    {
        private readonly WorkerPresenter _presenter;
        private List<TaskViewModel> _taskList;
        public WorkerController(AppDbContext context)
        {
            _taskList = new List<TaskViewModel>();
            _presenter = new WorkerPresenter(context, this);
        }

        public void ShowTasks(List<TaskViewModel> tasks)
        {
            _taskList = tasks;
        }

        public async Task<IActionResult> Dashboard()
        {
            await _presenter.LoadAvailableTasksAsync();
            return View(_taskList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTasks()
        {
            await _presenter.LoadAvailableTasksAsync();

            var result = _taskList.Select(t => new
            {
                t.Id,
                t.Title,
                t.Description,
                t.Location,
                DateTime = t.DateTime.ToString("g"),
                Deposit = t.DepositAmount,
                t.Status
            });

            return Json(result);
        }
    }
}
