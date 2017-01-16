using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;


namespace AuthServer.Repositories
{
    public class GiftCardRepository : IGiftCardRepository
    {
        ApplicationDbContext db;
        public GiftCardRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(GiftCard giftCard)
        {
            db.GiftCards.Add(giftCard);
            db.SaveChanges();
            return giftCard.Id;
        }

        public bool Delete(int id)
        {
            var gc = db.GiftCards.Find(id);
            if (gc == null) throw new NullReferenceException();
            db.GiftCards.Remove(gc);
            db.SaveChanges();
            return true;
        }

        public GiftCard Get(int id)
        {
            var gc = db.GiftCards.Find(id);
            if (gc == null) throw new NullReferenceException();
            return gc;
        }

        public IEnumerable<GiftCard> GetAll()
        {
            var gc = db.GiftCards.ToList();
            return gc;
        }

        public GiftCard Update(int id, GiftCard updated)
        {
            var gc = db.GiftCards.Find(id);
            if (gc == null) throw new NullReferenceException();
            gc.Amount = updated.Amount;
            //gc.CustomerId = updated.CustomerId;
            //gc.Deleted = updated.Deleted;
            gc.ExpiryDate = updated.ExpiryDate;
            gc.GiftCardTypeId = updated.GiftCardTypeId;
            gc.IssueDate = updated.IssueDate;
            gc.Notes = updated.Notes;
            gc.Number = updated.Number;
            gc.UpdatedAt = updated.UpdatedAt;
            gc.UpdatedBy = updated.UpdatedBy;
            db.SaveChanges();
            return gc;
        }
    }
}
