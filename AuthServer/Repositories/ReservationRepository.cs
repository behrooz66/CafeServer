using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private ApplicationDbContext db;
        public ReservationRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public bool Archive(int id)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
            LogHistory(r);
            r.Deleted = true;
            db.SaveChanges();
            return true;
        }

        public bool Unarchive(int id)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();

            // create history record
            LogHistory(r);

            r.Deleted = false;
            db.SaveChanges();
            return true;
        }

        public int Create(Reservation reservation)
        {
            db.Reservations.Add(reservation);
            db.SaveChanges();
            return reservation.Id;
        }

        public bool Delete(int id)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
            ClearHistory(r);
            db.Reservations.Remove(r);
            db.SaveChanges();
            return true;
        }

        public Reservation Get(int id)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
            return r;
        }

        public IEnumerable<Reservation> GetByCustomer(int customerId, bool includeDeleted = false)
        {
            var reservations = db.Reservations
                        .Where(r => r.Id == customerId);
            if (!includeDeleted)
                reservations = reservations.Where(r => !r.Deleted);

            var result = reservations
                        .OrderByDescending(r => r.Date)
                        .ToList()
                        .Select(r =>
                        {
                            r.Customer = null;
                            r.ReservationStatus = db.ReservationStatuses.Single(s => s.Id == r.ReservationStatusId);
                            return r;
                        });
            return result;
        }

        public IEnumerable<Reservation> GetByRestaurant(int restaurantId)
        {
            List<int> cIds = db.Customers
                        .Where(c => c.RestaurantId == restaurantId)
                        .Select(c => c.Id).ToList();
            var reservations = db.Reservations
                        .Where(r => cIds.Contains(r.CustomerId))
                        .Where(r => !r.Deleted)
                        .OrderByDescending(r => r.Date)
                        .ToList()
                        .Select(r =>
                        {
                            r.Customer = null;
                            r.ReservationStatus = db.ReservationStatuses.Single(s => s.Id == r.ReservationStatusId);
                            return r;
                        });
            return reservations;
        }

        public Reservation Update(int id, Reservation updated)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
            LogHistory(r);
            //r.CustomerId = updated.CustomerId;
            r.Date = updated.Date;
            //r.Deleted = updated.Deleted;
            r.Notes = updated.Notes;
            r.NumberOfPeople = updated.NumberOfPeople;
            r.ReservationStatusId = updated.ReservationStatusId;
            r.Revenue = updated.Revenue;
            r.Table = updated.Table;
            r.Time = updated.Time;
            r.UpdatedAt = updated.UpdatedAt;
            r.UpdatedBy = updated.UpdatedBy;
            db.SaveChanges();
            return r;
        }

        // *** DEALING WITH HISTORY
        public IEnumerable<ReservationHistory> GetHistory(int id)
        {
            var h = db.ReservationHistories.Where(rr => rr.ReservationId == id).ToList();
            return h;
        }

        private void LogHistory(Reservation r)
        {
            var h = new ReservationHistory()
            {
                Date = r.Date,
                Deleted = r.Deleted,
                Notes = r.Notes,
                NumberOfPeople = r.NumberOfPeople,
                ReservationId = r.Id,
                ReservationStatus = db.ReservationStatuses.Single(s => s.Id == r.ReservationStatusId).Status,
                Revenue = r.Revenue,
                Table = r.Table,
                Time = r.Time,
                UpdatedAt = r.UpdatedAt,
                UpdatedBy = r.UpdatedBy
            };
            db.ReservationHistories.Add(h);
            db.SaveChanges();
        }

        private void ClearHistory(Reservation r)
        {
            var h = db.ReservationHistories.Where(rr => rr.ReservationId == r.Id).ToList();
            db.ReservationHistories.RemoveRange(h);
        }
    }
}
