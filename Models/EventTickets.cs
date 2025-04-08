using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketWave.Models;

public class EventTickets
{
    [Key]
    public int EventId { get; set; }
    public string EventListUserID { get; set; } = string.Empty; // Seller ID
    [ForeignKey("EventListUserID")]
    public User? ListingUser { get; set; }

    public string? EventBuyerID { get; set; } // Buyer ID

    //[ForeignKey("EventListUserID")]
    //public User Seller { get; set; }  // ✅ Foreign key reference

    [Required]
    [Display(Name = "Listing Type (Buy or sell)")]
    public string ListingType { get; set; } = "Sell";

    [Display(Name = "When?")]
    [DataType(DataType.Date)]
    public DateTime EventDateTime { get; set; }

    [Display(Name = "Event Name")]
    public string? EventName { get; set; }

    [Display(Name = "Where?")]
    public string? EventLocation { get;set; }

    [Display(Name = "How much?")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EventTicketPrice { get; set; }

    [Display(Name = "How many tickets?")]
    public int EventNumOfTicket { get; set; }

    [Display(Name = "email address that will be used by others to contact you")]
    public string? EventUserContactEmail { get; set; }

    [Display(Name = "Enter an informative description of the event")]
    public string? EventDescription { get; set; }

    public bool EventBuyOfferAccepted { get; set; }

    public ICollection<EventOffer> EventOffers { get; set; } = new List<EventOffer>();

    // 🆕 New image upload support
    [Display(Name = "Uploaded Image")]
    public string? ImageFileName { get; set; }

}