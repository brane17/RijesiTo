using RijesiTo.ViewModels;

namespace RijesiTo.Interfaces
{
    public interface IWorkerTaskView
    {
       void ShowTasks(List<TaskViewModel> tasks);
    }
}
