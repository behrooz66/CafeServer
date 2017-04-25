using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        int Create(Order order);
        Order Update(int id, Order updated);
        bool Delete(int id);
        bool Archive(int id);
        //IEnumerable<Order> GetAll();
        IEnumerable<Order> GetByCustomer(int customerId);
        IEnumerable<Order> GetByRestaurant(int restaurantId);

        IEnumerable<OrderHistory> GetHistory(int id);

        Order Get(int id);
    }
}
