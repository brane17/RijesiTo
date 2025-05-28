using TaskStatus = RijesiTo.Models.TaskStatus;
namespace RijesiTo.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        
        public DateTime DateTime { get; set; }
        public double DepositAmount { get; set; } = 0.0; // Standardan vrijednost za iznos
        public TaskStatus Status { get; set; }

        public bool AllowReview { get; set; }
    }
}
