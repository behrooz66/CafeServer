using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IReservationStatusRepository
    {
        int Create(ReservationStatus reservationStatus);
        ReservationStatus Update(int id, ReservationStatus updated);
        bool Delete(int id);
        IEnumerable<ReservationStatus> GetAll();
        ReservationStatus Get(int id);
    }
}
