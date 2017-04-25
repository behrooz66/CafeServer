using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class OrderHistory
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public string OrderType { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
