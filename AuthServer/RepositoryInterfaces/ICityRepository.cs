using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    interface ICityRepository
    {
        int Create(City city);
        City Update(int id, City updatedCity);
        bool Delete(int id);
        IEnumerable<City> GetAll();
        City Get(int id);
    }
}
