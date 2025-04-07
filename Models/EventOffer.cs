using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketWave.Models
{
    public class EventOffer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OfferedByUserId { get; set; } = string.Empty;

        [ForeignKey("OfferedByUserId")]
        public User? OfferedByUser { get; set; }

        [Required]
        public int EventId { get; set; }

        public DateTime OfferDate { get; set; } = DateTime.UtcNow;

        public OfferStatus Status { get; set; } = OfferStatus.Pending;

        // Navigation (optional)
        public EventTickets? Event { get; set; }

    }
}
