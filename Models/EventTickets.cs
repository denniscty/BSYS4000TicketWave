using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketWave.Models;

public class EventTickets
{
    [Key]
    public int EventId { get; set; }
    public int EventListUserID { get; set; }  // Seller ID

    //[ForeignKey("EventListUserID")]
    //public User Seller { get; set; }  // ✅ Foreign key reference

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
    public string? EventUserContactEmail { get; set; }
    [Display(Name = "Enter an informative description of the event")]
    public string? EventDescription { get; set; }
    public int? EventBuyerID { get; set; }
    public bool EventBuyOfferAccepted { get; set; }
}