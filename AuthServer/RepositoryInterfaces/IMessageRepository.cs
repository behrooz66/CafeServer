using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;

namespace AuthServer.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        int Create(Message message);
        bool Delete(string messageId);
        bool Delete(int id);
        Message GetById(int id);
        IEnumerable<Message> GetByMessageId(string messageId);
        IEnumerable<Message> GetAllBySenderId(string senderId);
        IEnumerable<Message> GetAllByReceiverId(string receiverId);
        IEnumerable<User> GetReceivers(string messageId);
        User GetSender(string messageId);
        void MarkAsRead(int id);
        void MarkAsUnread(int id);
    }
}
