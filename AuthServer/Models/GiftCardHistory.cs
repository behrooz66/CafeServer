using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class GiftCardHistory
    {
        [Key]
        public int Id { get; set; }
        public int GiftCardId { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public string GiftCardType { get; set; }

        [ForeignKey("GiftCardId")]
        public virtual GiftCard GiftCard { get; set; }
    }
}
