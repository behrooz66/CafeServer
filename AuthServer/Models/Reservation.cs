using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime Date { get; set; }
        [MaxLength(5), MinLength(5)]
        public string Time { get; set; }
        public string Table { get; set; }
        public string Notes { get; set; }
        public decimal? Revenue { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }

        
        public int ReservationStatusId { get; set; }
        [ForeignKey("ReservationStatusId")]
        public virtual ReservationStatus ReservationStatus { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }

    public class ReservationStatus
    {
        [Key]
        public int Id { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        //public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
