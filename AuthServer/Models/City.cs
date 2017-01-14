using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double NWLat { get; set; }
        public double NELat { get; set; }
        public double NWLon { get; set; }
        public double NELon { get; set; }
        public double SWLat { get; set; }
        public double SWLon { get; set; }
        public double SELat { get; set; }
        public double SELon { get; set; }

        public int ProvinceId { get; set; }
        [ForeignKey("ProvinceId")]
        public virtual Province Province { get; set; }
    }
}
