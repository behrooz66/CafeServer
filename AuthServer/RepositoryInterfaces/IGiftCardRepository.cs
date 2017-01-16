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
        IEnumerable<GiftCard> GetAll();
        GiftCard Get(int id);
    }
}
