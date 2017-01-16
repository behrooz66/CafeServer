using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Infrastructure;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;

namespace AuthServer.Repositories
{
    public class CountryRepository:ICountryRepository
    {
        private ApplicationDbContext db;
        public CountryRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(Country country)
        {
            db.Countries.Add(country);
            db.SaveChanges();
            return country.Id;
        }

        public bool Delete(int id)
        {
            var c = db.Countries.Find(id);
            if (c == null) throw new NullReferenceException();
            db.Countries.Remove(c);
            db.SaveChanges();
            return true;
        }

        public Country Get(int id)
        {
            var c = db.Countries.Find(id);
            if (c == null) throw new NullReferenceException();
            return c;
        }

        public IEnumerable<Country> GetAll()
        {
            var c = db.Countries.ToList();
            return c;
        }

        public Country Update(int id, Country updatedCountry)
        {
            var c = db.Countries.Find(id);
            if (c == null) throw new NullReferenceException();
            c.Name = updatedCountry.Name;
            db.SaveChanges();
            return c;
        }
    }
}
