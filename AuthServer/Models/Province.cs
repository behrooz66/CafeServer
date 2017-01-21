using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class Province
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        //public virtual ICollection<City> Cities { get; set; }
    }
}
