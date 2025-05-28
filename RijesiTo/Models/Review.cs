using System.ComponentModel.DataAnnotations;

namespace RijesiTo.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TaskId { get; set; } // Strani ključ za zadatak

        public Task? Task { get; set; } // Svojstvo navigacije do zadatka

        [Required]
        public int UserId { get; set; } // Strani ključ za korisnika (radnik/klijent)
        public User? User { get; set; } // Svojstvo navigacije do korisnika
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Ocjena od 5
        [Required]
        public required string Comment { get; set; } // Komentar recenzije
        [Required]
        public DateTime Date { get; set; } = DateTime.Now; // Datum recenzije
    }

}
