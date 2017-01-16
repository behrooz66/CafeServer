using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IGiftCardTypeRepository
    {
        int Create(GiftCardType giftCardType);
        GiftCardType Update(int id, GiftCardType updated);
        bool Delete(int id);
        IEnumerable<GiftCardType> GetAll();
        GiftCardType Get(int id);
    }
}
