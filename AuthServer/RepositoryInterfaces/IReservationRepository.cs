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
        bool Archive(int id);
        IEnumerable<Reservation> GetByCustomer(int customerId);
        IEnumerable<Reservation> GetByRestaurant(int restaurantId);

        IEnumerable<ReservationHistory> GetHistory(int id);
        Reservation Get(int id);
    }
}
