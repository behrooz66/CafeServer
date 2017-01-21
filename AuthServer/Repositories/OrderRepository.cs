using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext db;
        public OrderRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public bool Archive(int id)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();
            o.Deleted = true;
            db.SaveChanges();
            return true;
        }

        public int Create(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.Id;
        }

        public bool Delete(int id)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();
            db.Orders.Remove(o);
            db.SaveChanges();
            return true;
        }

        public Order Get(int id)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();
            return o;
        }

        public IEnumerable<Order> GetByCustomer(int customerId)
        {
            var orders = db.Orders
                         .Where(o => o.CustomerId == customerId)
                         .Where(o => !o.Deleted)
                         .ToList();
            return orders;
        }

        public IEnumerable<Order> GetByRestaurant(int restaurantId)
        {
            List<int> cIds = db.Customers.Where(c => c.RestaurantId == restaurantId)
                            .Select(x => x.Id).ToList();
            var orders = db.Orders
                         .Where(o => cIds.Contains(o.CustomerId))
                         .Where(o => !o.Deleted)
                         .ToList();
            return orders;
        }

        public Order Update(int id, Order updated)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();
            //o.CustomerId = updated.CustomerId;
            o.Date = updated.Date;
            //o.Deleted = updated.Deleted;
            o.Notes = updated.Notes;
            o.OrderTypeId = updated.OrderTypeId;
            o.Price = updated.Price;
            o.UpdatedAt = o.UpdatedAt;
            o.UpdatedBy = updated.UpdatedBy;
            db.SaveChanges();
            return o;
        }
    }
}
