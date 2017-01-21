using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class GiftCard
    {
        [Key]
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Notes { get; set; }

        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }

        [Required]
        public int GiftCardTypeId { get; set; }
        [ForeignKey("GiftCardTypeId")]
        public virtual GiftCardType GiftCardType { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }

    public class GiftCardType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }

        //public virtual ICollection<GiftCardType> GiftCards { get; set; }
    }
}
