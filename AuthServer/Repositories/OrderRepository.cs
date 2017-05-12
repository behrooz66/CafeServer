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

            // create history record
            LogHistory(o);

            o.Deleted = true;
            db.SaveChanges();
            return true;
        }

        public bool Unarchive(int id)
        {
            var r = db.Orders.Find(id);
            if (r == null) throw new NullReferenceException();

            // create history record
            LogHistory(r);

            r.Deleted = false;
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

            // clear history
            ClearHistory(o);

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

        public IEnumerable<Order> GetByCustomer(int customerId, bool includeDeleted = false)
        {
            var orders = db.Orders.Where(o => o.CustomerId == customerId);
            if (!includeDeleted)
                orders = orders.Where(o => !o.Deleted);

            var result = orders.ToList()
                               .OrderByDescending(o => o.Date)
                               .Select(c =>
                               {
                                   c.Customer = null;
                                   c.OrderType = db.OrderTypes.Single(t => t.Id == c.OrderTypeId);
                                   return c;
                               });
            return result;
        }

        public IEnumerable<Order> GetByRestaurant(int restaurantId)
        {
            List<int> cIds = db.Customers.Where(c => c.RestaurantId == restaurantId)
                            .Select(x => x.Id).ToList();
            var orders = db.Orders
                         .Where(o => cIds.Contains(o.CustomerId))
                         .Where(o => !o.Deleted)
                         .OrderByDescending(o => o.Date)
                         .ToList()
                         .Select(o =>
                         {
                             o.Customer = null;
                             o.OrderType = db.OrderTypes.Single(t => t.Id == o.OrderTypeId);
                             return o;
                         });
            return orders;
        }

        public Order Update(int id, Order updated)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();

            // creating history record
            LogHistory(o);

            // do the update
            o.Date = updated.Date;
            o.Notes = updated.Notes;
            o.OrderTypeId = updated.OrderTypeId;
            o.Price = updated.Price;
            o.UpdatedAt = o.UpdatedAt;
            o.UpdatedBy = updated.UpdatedBy;
            db.SaveChanges();
            return o;
        }


        // **** DEALING WITH HISTORY

        public IEnumerable<OrderHistory> GetHistory(int id)
        {
            var h = db.OrderHistories.Where(oh => oh.OrderId == id).ToList();
            return h;
        }

        private void LogHistory(Order o)
        {
            var oh = new OrderHistory()
            {
                OrderId = o.Id,
                Date = o.Date,
                Deleted = o.Deleted,
                Notes = o.Notes,
                OrderType = db.OrderTypes.Single(ot => ot.Id == o.OrderTypeId).Type,
                Price = o.Price,
                UpdatedAt = o.UpdatedAt,
                UpdatedBy = o.UpdatedBy
            };
            db.OrderHistories.Add(oh);
            db.SaveChanges();
        }

        private void ClearHistory(Order o)
        {
            var ohs = db.OrderHistories.Where(oh => oh.OrderId == o.Id).ToList();
            db.OrderHistories.RemoveRange(ohs);
            db.SaveChanges();
        }

    }
}
