using RijesiTo.ViewModels;
using Task = RijesiTo.Models.Task;
namespace RijesiTo.Interfaces
{
    public interface ITaskView
    {
        TaskViewModel TaskViewModel { get; set; }
        void DisplayTasks(List<TaskViewModel> tasks);
        void ShowTask(TaskViewModel task);

        void DisplayAcceptedTasksForWorker(List<TaskViewModel> tasks);


    }
}
