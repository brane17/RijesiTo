using System.Threading.Tasks;

namespace RijesiTo.Models
{

    public class Offer
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; } // Svojstvo navigacije za zadatak (Task)
        public int UserId { get; set; } //kljuc za User (Worker)
        public User User { get; set; } // Svojstvo navigacije User (Worker)
        public DateTime OfferDate { get; set; }
        public OfferStatus OfferStatus { get; set; } = OfferStatus.Pending;
    }

    public enum OfferStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
