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
            r.Deleted = true;
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

        public IEnumerable<Reservation> GetByCustomer(int customerId)
        {
            var reservations = db.Reservations
                        .Where(r => r.Id == customerId)
                        .Where(r => !r.Deleted);
            return reservations;
        }

        public IEnumerable<Reservation> GetByRestaurant(int restaurantId)
        {
            List<int> cIds = db.Customers
                        .Where(c => c.RestaurantId == restaurantId)
                        .Select(c => c.Id).ToList();
            var reservations = db.Reservations
                        .Where(r => cIds.Contains(r.CustomerId))
                        .Where(r => !r.Deleted);
            return reservations;
        }

        public Reservation Update(int id, Reservation updated)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
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
    }
}
