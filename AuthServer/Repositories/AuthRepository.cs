using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AuthServer.Infrastructure;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;

namespace AuthServer.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private ApplicationDbContext db;

        public AuthRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public List<UserClaim> GetClaims(string userId)
        {
            var claims = db.UserClaims.Where(u => u.UserId == userId).ToList();
            return claims;
        }

        public int[] GetDefaultLocationIds(string userId)
        {
            // returns an array of integer, including (in order) cityId, provinceId and countryId
            var restaurantId = db.Users.Where(u => u.Id == userId).FirstOrDefault().RestaurantId;
            var cityId = db.Restaurants.Where(r => r.Id == restaurantId).FirstOrDefault().CityId;
            var provinceId = db.Cities.Where(c => c.Id == cityId).FirstOrDefault().ProvinceId;
            var countryId = db.Provinces.Where(p => p.Id == provinceId).FirstOrDefault().CountryId;
            var result = new int[] { cityId, provinceId, countryId, restaurantId };
            return result;
        }

        public string[] GetDefaultLocationNames(string userId)
        {
            var restaurantId = db.Users.Where(u => u.Id == userId).FirstOrDefault().RestaurantId;
            var cityId = db.Restaurants.Where(r => r.Id == restaurantId).FirstOrDefault().CityId;
            var provinceId = db.Cities.Where(c => c.Id == cityId).FirstOrDefault().ProvinceId;
            var countryId = db.Provinces.Where(p => p.Id == provinceId).FirstOrDefault().CountryId;
            var cityName = db.Cities.Single(c => c.Id == cityId).Name;
            var provinceName = db.Provinces.Single(p => p.Id == provinceId).Name;
            var countryName = db.Countries.Single(c => c.Id == countryId).Name;
            var results = new string[] { cityName, provinceName, countryName };
            return results;
        }

        public User GetUserById(string userId)
        {
            var user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            return user;
        }

        public User GetUserByUsername(string username)
        {
            var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            return user;
        }

        public bool ValidatePassword(string username, string password)
        {
            var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null) return false;
            if (user.Password == password) return true;
            return false;
        }
    }
}
