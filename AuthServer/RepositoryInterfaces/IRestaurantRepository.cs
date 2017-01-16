using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IRestaurantRepository
    {
        int Create(Restaurant restaurant);
        Restaurant Update(int id, Restaurant updated);
        bool Delete(int id);
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int id);
    }
}
