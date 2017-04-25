using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class ReservationHistory
    {
        [Key]
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Table { get; set; }
        public string Notes { get; set; }
        public decimal? Revenue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public string ReservationStatus { get; set; }
        
        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }

    }
}
