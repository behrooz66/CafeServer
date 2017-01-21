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

        public bool Archive(int id)
        {
            var g = db.GiftCards.Find(id);
            if (g == null) throw new NullReferenceException();
            g.Deleted = true;
            db.SaveChanges();
            return true;
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

        public IEnumerable<GiftCard> GetByCustomer(int customerId)
        {
            var giftcards = db.GiftCards
                            .Where(g => g.CustomerId == customerId)
                            .Where(g => !g.Deleted)
                            .ToList();
            return giftcards;
        }

        public IEnumerable<GiftCard> GetByRestaurant(int restaurantId)
        {
            List<int> cIds = db.Customers.Where(c => c.RestaurantId == restaurantId).Select(c => c.Id).ToList();
            var giftcards = db.GiftCards.Where(g => cIds.Contains(g.CustomerId))
                                        .Where(g => !g.Deleted);
            return giftcards;
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
