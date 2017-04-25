using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface ICustomerRepository
    {
        int Create(Customer customer);
        Customer Update(int id, Customer updated);
        bool Delete(int id);
        bool Archive(int id);
        //IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetByRestaurant(int restaurantId);

        IEnumerable<CustomerHistory> GetHistory(int id);
        Customer Get(int id);
    }
}
