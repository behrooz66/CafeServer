using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IReservationRepository
    {
        int Create(Reservation reservation);
        Reservation Update(int id, Reservation updated);
        bool Delete(int id);
        IEnumerable<Reservation> GetAll();
        Reservation Get(int id);
    }
}
