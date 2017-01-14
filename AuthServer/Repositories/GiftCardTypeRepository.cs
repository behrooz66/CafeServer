using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class GiftCardTypeRepository : IGiftCardTypeRepository
    {
        private ApplicationDbContext db;
        public GiftCardTypeRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(GiftCardType giftCardType)
        {
            db.GiftCardTypes.Add(giftCardType);
            return giftCardType.Id;
        }

        public bool Delete(int id)
        {
            var ot = db.GiftCardTypes.Find(id);
            if (ot == null) throw new NullReferenceException();
            db.GiftCardTypes.Remove(ot);
            return true;
        }

        public GiftCardType Get(int id)
        {
            var ot = db.GiftCardTypes.Find(id);
            if (ot == null) throw new NullReferenceException();
            return ot;
        }

        public IEnumerable<GiftCardType> GetAll()
        {
            var ot = db.GiftCardTypes.ToList();
            return ot;
        }

        public GiftCardType Update(int id, GiftCardType updated)
        {
            var ot = db.GiftCardTypes.Find(id);
            if (ot == null) throw new NullReferenceException();
            ot.Notes = updated.Notes;
            ot.Type = updated.Type;
            db.SaveChanges();
            return ot;
        }
    }
}
