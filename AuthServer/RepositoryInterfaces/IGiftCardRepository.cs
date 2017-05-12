using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IGiftCardRepository
    {
        int Create(GiftCard giftCard);
        GiftCard Update(int id, GiftCard updated);
        bool Delete(int id);
        bool Archive(int id);
        IEnumerable<GiftCard> GetByCustomer(int customerId, bool includeDeleted=false);
        IEnumerable<GiftCard> GetByRestaurant(int restaurantId);

        IEnumerable<GiftCardHistory> GetHistory(int id);
        bool Unarchive(int id);
        GiftCard Get(int id);
    }
}
