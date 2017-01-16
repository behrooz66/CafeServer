using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface ICountryRepository
    {
        int Create(Country country);
        Country Update(int id, Country updatedCountry);
        bool Delete(int id);
        IEnumerable<Country> GetAll();
        Country Get(int id);
    }
}
