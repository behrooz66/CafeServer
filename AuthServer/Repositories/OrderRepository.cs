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

        public int Create(Order order)
        {
            db.Orders.Add(order);
            return order.Id;
        }

        public bool Delete(int id)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();
            db.Orders.Remove(o);
            return true;
        }

        public Order Get(int id)
        {
            var o = db.Orders.Find(id);
            if (o == null) throw new NullReferenceException();
            return o;
        }

        public IEnumerable<Order> GetAll()
        {
            var o = db.Orders.ToList();
            return o;
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
