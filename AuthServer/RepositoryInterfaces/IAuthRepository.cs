using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IAuthRepository
    {
        User GetUserById(string userId);
        User GetUserByUsername(string username);
        bool ValidatePassword(string username, string password);
        List<UserClaim> GetClaims(string userId);
    }
}
