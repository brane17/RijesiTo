using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.Models;
using RijesiTo.Presenters;
using RijesiTo.ViewModels;
using System.Threading.Tasks;
using Task = RijesiTo.Models.Task;
namespace RijesiTo.Controllers
{
    public class TaskController : Controller, ITaskView
    {
        private readonly TaskPresenter _presenter;
        public TaskViewModel TaskViewModel { get; set; }

        public TaskController(AppDbContext context)
        {
            _presenter = new TaskPresenter(context, this);
        }

        public List<TaskViewModel> TasksToDisplay { get; set; }

        public List<TaskViewModel> AcceptedTasksToDisplay { get; set; }

        public TaskViewModel TaskToDisplay { get; set; }

        public void DisplayTasks(List<TaskViewModel> tasks)
        {
            TasksToDisplay = tasks;
        }

        public void DisplayAcceptedTasksForWorker(List<TaskViewModel> tasks)
        {
            AcceptedTasksToDisplay = tasks;
        }

        public void ShowTask(TaskViewModel task)
        {
            TaskToDisplay = task;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTasks()
        {
            string? userRole = HttpContext.Session.GetString("UserRole");
            if (userRole == UserRole.Client.ToString())
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                await _presenter.LoadTasksAsync(userId.Value);
            }
            else
            {
                await _presenter.LoadTasksAsync();
            }

            return Json(TasksToDisplay);
        }

        public IActionResult AcceptedTasks()
        {
             return View();
        }


        public async Task<IActionResult> GetAcceptedTasks()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { error = "User is not logged in." });
            }

            await _presenter.LoadAcceptedTasksAsync(userId.Value);
            return Json(AcceptedTasksToDisplay);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            await _presenter.LoadTaskAsync(id, userId.Value);
    
            return View(TaskToDisplay);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Task task)
        {
            // Zanemari korisnika iz ModelState-a.
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    task.UserId = userId.Value; // Postavi korisnički ID iz sesije
                }
                else
                {
                    ModelState.AddModelError("", "User is not logged in.");
                    return View(task);
                }
                await _presenter.CreateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _presenter.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Task task)
        {
            // Zanemari korisnika iz ModelState-a.
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                await _presenter.UpdateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _presenter.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _presenter.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MarkFinished([FromBody] int taskId)
        {
            var workerId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _presenter.MarkTaskFinishedAsync(taskId, workerId);
            return Ok(new { success = true, newStatus = "Finished" });
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCompletion([FromBody] int taskId)
        {
            var clientId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _presenter.ApproveTaskCompletionAsync(taskId, clientId);
            return Ok(new { success = true, newStatus = "Completed" });
        }

    }
}
