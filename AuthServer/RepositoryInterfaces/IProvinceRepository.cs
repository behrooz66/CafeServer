using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IProvinceRepository
    {
        int Create(Province province);
        Province Update(int id, Province updatedProvince);
        bool Delete(int id);
        IEnumerable<Province> GetAll();
        Province Get(int id);
    }
}
