using Microsoft.EntityFrameworkCore;
using RijesiTo.Data;
using RijesiTo.Interfaces;
using RijesiTo.ViewModels;
using TaskEntity = RijesiTo.Models.Task;

namespace RijesiTo.Presenters
{
    public class TaskPresenter
    {
        private readonly AppDbContext _context;
        private readonly ITaskView _view;

        public TaskPresenter(AppDbContext context, ITaskView view)
        {
            _context = context;
            _view = view;
        }

        public async Task LoadTasksAsync(int userId = 0)
        {
            List<TaskEntity> tasks = new List<TaskEntity>();
            if (userId > 0)
            {
                tasks = await _context.Tasks
                    .Where(t => t.UserId == userId)
                    .ToListAsync();
            }
            else
            {
                tasks = await _context.Tasks.ToListAsync();
            }

            var taskViewModels = tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Location = t.Location,
                Status = t.Status
            }).ToList();

            _view.DisplayTasks(taskViewModels);
        }

        public async Task LoadAcceptedTasksAsync(int userId)
        {
            List<TaskEntity> tasks = new List<TaskEntity>();
            var acceptedTaskIds = await _context.Offers
                .Where(o => o.UserId == userId && o.OfferStatus == Models.OfferStatus.Accepted)
                .Select(x=>x.TaskId).ToListAsync();
            if (acceptedTaskIds.Count > 0)
            {
                tasks = await _context.Tasks
                    .Where(t => acceptedTaskIds.Contains(t.Id))
                    .ToListAsync();
            }

            var taskViewModels = tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Location = t.Location,
                Status = t.Status
            }).ToList();

            _view.DisplayAcceptedTasksForWorker(taskViewModels);
        }

        public async Task LoadTaskAsync(int id, int loggedInUserId)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                var review = await _context.Reviews.FirstOrDefaultAsync(r => r.TaskId == id && r.UserId == loggedInUserId);
                
                var taskViewModel = new TaskViewModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    DateTime = task.DateTime,
                    Location = task.Location,
                    DepositAmount = task.DepositAmount,
                    Status = task.Status,
                    AllowReview = review == null ? true : false
                };
                _view.ShowTask(taskViewModel);
            }
        }

        public async Task CreateTaskAsync(TaskEntity taskNew)
        {
            var task = new TaskEntity
            {
                UserId = taskNew.UserId,
                Title = taskNew.Title,
                Description = taskNew.Description,
                DateTime = taskNew.DateTime,
                Location = taskNew.Location,
                DepositAmount = taskNew.DepositAmount,
                Status = Models.TaskStatus.NotStarted
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskEntity taskToUpdate)
        {
            var task = await _context.Tasks.FindAsync(taskToUpdate.Id);
            if (task != null)
            {
                task.Title = taskToUpdate.Title;
                task.Description = taskToUpdate.Description;
                task.DateTime = taskToUpdate.DateTime;
                task.Location = taskToUpdate.Location;
                task.DepositAmount = taskToUpdate.DepositAmount;
                task.Status = taskToUpdate.Status;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TaskEntity?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task MarkTaskFinishedAsync(int taskId, int workerId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                task.Status = Models.TaskStatus.Finished;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ApproveTaskCompletionAsync(int taskId, int clientId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null && task.Status == Models.TaskStatus.Finished)
            {
                task.Status = Models.TaskStatus.Completed;
                await _context.SaveChangesAsync();
            }
        }
    }
}
