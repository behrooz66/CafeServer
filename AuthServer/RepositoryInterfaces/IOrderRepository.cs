using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    interface IOrderRepository
    {
        int Create(Order order);
        Order Update(int id, Order updated);
        bool Delete(int id);
        IEnumerable<Order> GetAll();
        Order Get(int id);
    }
}
