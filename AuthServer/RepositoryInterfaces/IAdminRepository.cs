using AuthServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.RepositoryInterfaces
{
    public interface IAdminRepository
    {
        // User Management
        string AddUser(string name, string username, string hashedPassword, int restaurantId);
        void DeleteUser(string id);
        User UpdateUser(string id, User user);

        IEnumerable<User> GetUsersByRestaurant(int restaurantId);
        User GetUser(string id);
        bool UserExists(string username);

        // Support Tickets
        // todo: later!
    }
}
