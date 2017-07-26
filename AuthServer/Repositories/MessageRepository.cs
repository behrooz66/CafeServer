using AuthServer.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using AuthServer.Infrastructure;

namespace AuthServer.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private ApplicationDbContext db;
        public MessageRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        
        public int Create(Message message)
        {
            db.Messages.Add(message);
            db.SaveChanges();
            return message.Id;
        }

        public bool Delete(int id)
        {
            var m = db.Messages.Where(me => me.Id == id).FirstOrDefault();
            if (m == null) throw new NullReferenceException();
            db.Messages.Remove(m);
            db.SaveChanges();
            return true;
        }

        public bool Delete(string messageId)
        {
            throw new NotImplementedException();
        }

        public Message Get(int id)
        {
            var m = db.Messages.Where(me => me.Id == id).FirstOrDefault();
            if (m == null) throw new NullReferenceException();
            return m;
        }

        public IEnumerable<Message> GetAllByReceiverId(string receiverId)
        {
            var messages = db.Messages.Where(m => m.ReceiverId == receiverId).OrderByDescending(m => m.Id).ToList();
            return messages;
        }

        public IEnumerable<Message> GetAllBySenderId(string senderId)
        {
            var messages = db.Messages.Where(m => m.SenderId == senderId).OrderByDescending(m => m.Id).ToList();
            return messages;
        }

        public Message GetById(int id)
        {
            var m = db.Messages.Where(me => me.Id == id).FirstOrDefault();
            if (m == null) throw new NullReferenceException();
            return m;
        }

        public IEnumerable<Message> GetByMessageId(string messageId)
        {
            var m = db.Messages.Where(me => me.MessageId == messageId).ToList();
            return m;
        }

        public IEnumerable<User> GetReceivers(string messageId)
        {
            var users = db.Users.Where(u => db.Messages.Where(m => m.MessageId == messageId).Select(m => m.ReceiverId).Contains(u.Id)).ToList();
            return users;
        }

        public User GetSender(string messageId)
        {
            var senderId = db.Messages.Where(m => m.MessageId == messageId).Select(m => m.SenderId).FirstOrDefault();
            var user = db.Users.Where(u => u.Id == senderId).FirstOrDefault();
            return user;
        }

        public void MarkAsRead(int id)
        {
            var m = db.Messages.Where(me => me.Id == id).FirstOrDefault();
            if (m == null) throw new NullReferenceException();
            m.Read = true;
            db.SaveChanges();
        }

        public void MarkAsUnread(int id)
        {
            var m = db.Messages.Where(me => me.Id == id).FirstOrDefault();
            if (m == null) throw new NullReferenceException();
            m.Read = false;
            db.SaveChanges();
        }

    }
}
