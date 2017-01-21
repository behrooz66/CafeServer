using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Infrastructure;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;

namespace AuthServer.Repositories
{
    public class CityRepository: ICityRepository
    {
        private ApplicationDbContext db;
        public CityRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(City city)
        {
            db.Cities.Add(city);
            db.SaveChanges();
            return city.Id;
        }

        public bool Delete(int id)
        {
            var c = db.Cities.Where(x => x.Id == id).FirstOrDefault();
            if (c == null) throw new NullReferenceException();
            db.Cities.Remove(c);
            db.SaveChanges();
            return true;
        }

        public City Get(int id)
        {
            var c = db.Cities.Where(x => x.Id == id).FirstOrDefault();
            if (c == null) throw new NullReferenceException();
            return c;
        }

        public IEnumerable<City> GetAll()
        {
            var cities = db.Cities.ToList();
            return cities;
        }

        public IEnumerable<City> GetByProvince(int provinceId)
        {
            var cities = db.Cities.Where(x => x.ProvinceId == provinceId).ToList();
            return cities;
        }

        public City Update(int id, City updatedCity)
        {
            var c = db.Cities.Where(x => x.Id == id).FirstOrDefault();
            if (c == null) throw new NullReferenceException();
            c.Lat = updatedCity.Lat;
            c.Lon = updatedCity.Lon;
            c.Name = updatedCity.Name;
            c.NELat = updatedCity.NELat;
            c.NELon = updatedCity.NELon;
            c.NWLat = updatedCity.NWLat;
            c.NWLon = updatedCity.NWLon;
            c.ProvinceId = updatedCity.ProvinceId;
            c.SELat = updatedCity.SELat;
            c.SELon = updatedCity.SELon;
            c.SWLat = updatedCity.SWLat;
            c.SWLon = updatedCity.SWLon;
            db.SaveChanges();
            return c;
        }
    }
}
