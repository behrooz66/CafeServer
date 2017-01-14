using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    interface IOrderTypeRepository
    {
        int Create(OrderType orderType);
        OrderType Update(int id, OrderType updated);
        bool Delete(int id);
        IEnumerable<OrderType> GetAll();
        OrderType Get(int id);
    }
}
