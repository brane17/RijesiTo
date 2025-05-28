using System.ComponentModel.DataAnnotations;

namespace RijesiTo.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public string ReviewerName { get; set; } = string.Empty;

        public DateTime ReviewDate { get; set; }

        public string ReviewerType { get; set; } = string.Empty; // "Radnik" ili "Klijent"

    }
}
