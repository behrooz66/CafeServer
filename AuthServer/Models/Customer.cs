using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cell { get; set; }
        public string Home { get; set; }
        public string Work { get; set; }
        public string OtherPhone { get; set; }
        public string Email { get; set; }

        [DefaultValue(false)]
        public bool NoAddress { get; set; }
        // this is for marking whether the address was physically locatable on the map
        public bool AddressFound { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string Notes { get; set; }

        [DefaultValue("2017-01-01")]
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public bool Deleted { get; set; }


        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<GiftCard> GiftCards { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
