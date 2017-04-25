using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class CustomerHistory
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Cell { get; set; }
        public string Home { get; set; }
        public string Work { get; set; }
        public string OtherPhone { get; set; }
        public string Email { get; set; }
        [DefaultValue(false)]
        public bool NoAddress { get; set; }
        public bool AddressFound { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public string City { get; set; }
        public string Restaurant { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
