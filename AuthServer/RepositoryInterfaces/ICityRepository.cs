using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface ICityRepository
    {
        int Create(City city);
        City Update(int id, City updatedCity);
        bool Delete(int id);
        IEnumerable<City> GetAll();
        IEnumerable<City> GetByProvince(int provinceId);
        City Get(int id);
    }
}
