using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class OrderTypeRepository : IOrderTypeRepository
    {
        private ApplicationDbContext db;
        public OrderTypeRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(OrderType orderType)
        {
            db.OrderTypes.Add(orderType);
            db.SaveChanges();
            return orderType.Id;
        }

        public bool Delete(int id)
        {
            var ot = db.OrderTypes.Find(id);
            if (ot == null) throw new NullReferenceException();
            db.OrderTypes.Remove(ot);
            db.SaveChanges();
            return true;
        }

        public OrderType Get(int id)
        {
            var ot = db.OrderTypes.Find(id);
            if (ot == null) throw new NullReferenceException();
            return ot;
        }

        public IEnumerable<OrderType> GetAll()
        {
            var ot = db.OrderTypes.ToList();
            return ot;
        }

        public OrderType Update(int id, OrderType updated)
        {
            var ot = db.OrderTypes.Find(id);
            if (ot == null) throw new NullReferenceException();
            ot.Notes = updated.Notes;
            ot.Type = updated.Type;
            db.SaveChanges();
            return ot;
        }
    }
}
