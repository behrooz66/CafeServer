using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.RepositoryInterfaces;
using AuthServer.Models;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class RestaurantRepository: IRestaurantRepository
    {
        private ApplicationDbContext db;
        public RestaurantRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(Restaurant restaurant)
        {
            db.Restaurants.Add(restaurant);
            return restaurant.Id;
        }

        public bool Delete(int id)
        {
            var res = db.Restaurants.Find(id);
            if (res == null) throw new NullReferenceException();
            db.Restaurants.Remove(res);
            return true;
        }

        public Restaurant Get(int id)
        {
            var res = db.Restaurants.Find(id);
            if (res == null) throw new NullReferenceException();
            return res;
        }

        public IEnumerable<Restaurant> GetAll()
        {
            var res = db.Restaurants.ToList();
            return res;
        }

        public Restaurant Update(int id, Restaurant updated)
        {
            var res = db.Restaurants.Find(id);
            if (res == null) throw new NullReferenceException();
            res.Address = updated.Address;
            res.CityId = updated.CityId;
            res.Name = updated.Name;
            res.Notes = updated.Notes;
            res.PostalCode = updated.PostalCode;
            res.Verified = updated.Verified;
            db.SaveChanges();
            return res;
        }
    }
}
