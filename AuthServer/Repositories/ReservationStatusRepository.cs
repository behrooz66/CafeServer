using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class ReservationStatusRepository : IReservationStatusRepository
    {
        private ApplicationDbContext db;
        public ReservationStatusRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public int Create(ReservationStatus reservationStatus)
        {
            db.ReservationStatuses.Add(reservationStatus);
            return reservationStatus.Id;
        }

        public bool Delete(int id)
        {
            var res = db.ReservationStatuses.Find(id);
            if (res == null) throw new NullReferenceException();
            db.ReservationStatuses.Remove(res);
            return true;
        }

        public ReservationStatus Get(int id)
        {
            var res = db.ReservationStatuses.Find(id);
            if (res == null) throw new NullReferenceException();
            return res;
        }

        public IEnumerable<ReservationStatus> GetAll()
        {
            var res = db.ReservationStatuses.ToList();
            return res;
        }

        public ReservationStatus Update(int id, ReservationStatus updated)
        {
            var res = db.ReservationStatuses.Find(id);
            if (res == null) throw new NullReferenceException();
            res.Notes = updated.Notes;
            res.Status = updated.Status;
            db.SaveChanges();
            return res;
        }
    }
}
