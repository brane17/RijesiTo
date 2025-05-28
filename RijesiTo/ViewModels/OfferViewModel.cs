using RijesiTo.Models;
using TaskStatus = RijesiTo.Models.TaskStatus;

namespace RijesiTo.ViewModels
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime OfferDate { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public TaskStatus TaskStatus { get; set; } = TaskStatus.InProgress;
        public bool IsAnyOfferAccepted { get; set; }

    }
}
