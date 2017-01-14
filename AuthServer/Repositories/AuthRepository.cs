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
