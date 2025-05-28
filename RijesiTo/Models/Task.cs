using System.ComponentModel.DataAnnotations;

namespace RijesiTo.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }//Strani ključ za usera
        public User User { get; set; } // Svojstvo navigacije user
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public required string Location { get; set; }
        [Required]
        public double DepositAmount { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
    }


    public enum TaskStatus
    {
        NotStarted, // zadani status nakon stvaranja zadatka
        InProgress, // postavlja sustav nakon što radnik prihvati ponudu
        Finished, // postavio radnik
        Completed, // postavio klijent nakon završetka
    }

}
