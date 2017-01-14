using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Notes { get; set; }
        public bool Verified { get; set; }

        public int CityId { get; set; }
        
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<User> Users { get; set; }
        
    }
}
