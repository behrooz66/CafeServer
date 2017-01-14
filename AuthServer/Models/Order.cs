using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }

        public int OrderTypeId { get; set; }
        [ForeignKey("OrderTypeId")]
        public virtual OrderType OrderType { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

    }

    public class OrderType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
