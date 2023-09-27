using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    [Table("venue_location")]
    public class MySqlTheater : MySqlModel
    {
        [Key]
        public int Venue_ID { get; set; }
        public string? Venue_Name { get; set;}
        public string? Country { get; set;}
        public string? Address { get; set;}
        public string? City { get; set;}
        public string? County { get; set;}
        public string? State { get; set;}
        public string? Postcode { get; set;}
        public float? Lattitude { get; set;}
        public float? Longitude { get; set;}
        public string? Venue_Type { get; set;}
        public string? Extra_Venue { get; set;}
        public int? Screens { get; set;}
        public string? Venue_Sound { get; set;}
        public string? Closed { get; set;}
        public string? Attributes { get; set;}
        public string? Seating { get; set;}
        public string? Comments { get; set;}
        public string? DMA_Market { get; set;}
        public string? Ticket_Adult { get; set;}
        public string? Ticket_Child { get; set;}
        public string? Ticket_Senior { get; set;}
        public string? Ticket_Online { get; set;}
        public string? Ticket_Bargain { get; set;}
        public string? Ticket_Comment { get; set;}
    }
}
