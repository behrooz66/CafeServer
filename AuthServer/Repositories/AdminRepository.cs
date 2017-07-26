using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private ApplicationDbContext db;

        public AdminRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public User UpdateUser(string id, User U)
        {
            var user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null) throw new NullReferenceException();
            user.Active = U.Active;
            user.Email = U.Email;
            user.MustChangePassword = U.MustChangePassword;
            user.Name = U.Name;
            user.Password = U.Password;
            user.RestaurantId = U.RestaurantId;
            user.Username = U.Username;
            user.Verified = U.Verified;
            db.SaveChanges();
            return user;
        }

        public string AddUser(string name, string username, string hashedPassword, int restaurantId)
        {
            var user = new User()
            {
                Active = true,
                Email = "",
                MustChangePassword = true,
                Name = name,
                Password = hashedPassword,
                RestaurantId = restaurantId,
                Username = username,
                Verified = true
            };
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public void DeleteUser(string id)
        {
            var user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null) throw new NullReferenceException();

            var messages = db.Messages.Where(m => m.ReceiverId == user.Id || m.SenderId == user.Id).ToList();
            db.Messages.RemoveRange(messages);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public User GetUser(string id)
        {
            var user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null) throw new NullReferenceException();
            return user;
        }

        public IEnumerable<User> GetUsersByRestaurant(int restaurantId)
        {
            var users = db.Users.Where(u => u.RestaurantId == restaurantId).ToList();
            return users;
        }

        public bool UserExists(string username)
        {
            var c = db.Users.Where(u => u.Username == username).Count();
            if (c > 0)
                return true;
            return false;
        }
    }


}
