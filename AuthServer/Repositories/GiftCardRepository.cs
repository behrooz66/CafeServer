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

            // Log History
            LogHistory(g);

            g.Deleted = true;
            db.SaveChanges();
            return true;
        }

        public bool Unarchive(int id)
        {
            var r = db.GiftCards.Find(id);
            if (r == null) throw new NullReferenceException();

            // create history record
            LogHistory(r);

            r.Deleted = false;
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

            //clear history
            ClearHistory(gc);

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

        public IEnumerable<GiftCard> GetByCustomer(int customerId, bool includeDeleted = false)
        {
            var giftcards = db.GiftCards
                            .Where(g => g.CustomerId == customerId);

            if (!includeDeleted)
                giftcards = giftcards.Where(g => !g.Deleted);

            var result =    giftcards
                            .OrderByDescending(g => g.IssueDate)
                            .ToList()
                            .Select(g =>
                            {
                                g.Customer = null;
                                g.GiftCardType = db.GiftCardTypes.Single(t => t.Id == g.GiftCardTypeId);
                                return g;
                            });
            return result;
        }

        public IEnumerable<GiftCard> GetByRestaurant(int restaurantId)
        {
            List<int> cIds = db.Customers.Where(c => c.RestaurantId == restaurantId).Select(c => c.Id).ToList();
            var giftcards = db.GiftCards.Where(g => cIds.Contains(g.CustomerId))
                                        .Where(g => !g.Deleted)
                                        .OrderByDescending(g => g.IssueDate)
                                        .ToList()
                                        .Select(g =>
                                        {
                                            g.Customer = null;
                                            g.GiftCardType = db.GiftCardTypes.Single(t => t.Id == g.GiftCardTypeId);
                                            return g;
                                        });
            return giftcards;
        }

        public GiftCard Update(int id, GiftCard updated)
        {
            var gc = db.GiftCards.Find(id);
            if (gc == null) throw new NullReferenceException();

            // log history
            LogHistory(gc);

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

        // *** DEALING WITH HISTORY

        public IEnumerable<GiftCardHistory> GetHistory(int id)
        {
            var h = db.GiftCardHistories.Where(hh => hh.GiftCardId == id).ToList();
            return h;
        }

        private void LogHistory(GiftCard g)
        {
            var h = new GiftCardHistory()
            {
                Amount = g.Amount,
                Deleted = g.Deleted,
                ExpiryDate = g.ExpiryDate,
                GiftCardId = g.Id,
                GiftCardType = db.GiftCardTypes.Single(t => t.Id == g.GiftCardTypeId).Type,
                IssueDate = g.IssueDate,
                Notes = g.Notes,
                Number = g.Number,
                UpdatedAt = g.UpdatedAt,
                UpdatedBy = g.UpdatedBy
            };
            db.GiftCardHistories.Add(h);
            db.SaveChanges();
        }

        private void ClearHistory(GiftCard g)
        {
            var h = db.GiftCardHistories.Where(hh => hh.GiftCardId == g.Id).ToList();
            db.GiftCardHistories.RemoveRange(h);
            db.SaveChanges();
        }
    }
}
