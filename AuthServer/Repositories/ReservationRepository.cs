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

        public int Create(Reservation reservation)
        {
            db.Reservations.Add(reservation);
            return reservation.Id;
        }

        public bool Delete(int id)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
            db.Reservations.Remove(r);
            return true;
        }

        public Reservation Get(int id)
        {
            var r = db.Reservations.Find(id);
            if (r == null) throw new NullReferenceException();
            return r;
        }

        public IEnumerable<Reservation> GetAll()
        {
            var r = db.Reservations.ToList();
            return r;
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
