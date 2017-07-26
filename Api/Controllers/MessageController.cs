using Api.Helpers;
using AuthServer.Models;
using AuthServer.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class MessageController: Controller
    {
        private IMessageRepository _rep;
        private IAuthRepository _auth;
        private IHelper _helper;
        private IAdminRepository _admin;
        private IRestaurantRepository _restaurant;
        private ICityRepository _city;
        public MessageController(IMessageRepository msg,
                                 IAuthRepository auth,
                                 IHelper helper,
                                 IAdminRepository admin,
                                 IRestaurantRepository restaurant,
                                 ICityRepository city
                                )
        {
            this._rep = msg;
            this._auth = auth;
            this._helper = helper;
            this._admin = admin;
            this._restaurant = restaurant;
            this._city = city;
        }

        [HttpGet]
        [Route("getUsers")]
        [Authorize]
        public ActionResult GetUsers()
        {
            var restaurantId = _helper.GetUserEntity(User, _auth).RestaurantId;
            var users = _admin.GetUsersByRestaurant(restaurantId).Where(u => u.Active && u.Verified).Select(u => new
            {
                u.Id,
                u.Name,
                u.Username,
                u.Active
            });
            return Ok(users);
        }


        [HttpGet]
        [Route("getBySender/{senderId}")]
        [Authorize]
        public ActionResult GetBySender(string senderId)
        {
            if (senderId != this._helper.GetUserEntity(User, _auth).Id)
                return Unauthorized();

            var result = this._rep.GetAllBySenderId(senderId).Select(x => new
            {
                x.Id, 
                x.MessageId,
                x.Read,
                x.Subject,
                x.Body,
                x.CreateDate,
                x.CreateTime,
                Sender = _rep.GetSender(x.MessageId).Name,
                Receivers = _rep.GetReceivers(x.MessageId).Select(r => r.Name)
            }).GroupBy(x => x.MessageId);

            // this will return multipe identical records because of multiple receivers, therefore we eliminate the extra ones by the following:
            var finalResult = new List<object>();
            foreach (IEnumerable<object> x in result)
            {
                finalResult.Add(x.FirstOrDefault());
            }

            return Ok(finalResult);
        }

        [HttpGet]
        [Route("getByReceiver/{receiverId}")]
        [Authorize]
        public ActionResult GetByReceiver(string receiverId)
        {
            if (receiverId != this._helper.GetUserEntity(User, _auth).Id)
                return Unauthorized();

            var result = this._rep.GetAllByReceiverId(receiverId).Select(x => new
            {
                x.Id,
                x.MessageId,
                x.Read,
                x.Subject,
                x.Body,
                x.CreateTime,
                x.CreateDate,
                Sender = _rep.GetSender(x.MessageId).Name,
                Receivers = _rep.GetReceivers(x.MessageId).Select(r => r.Name)
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("getNewMessagesCount")]
        [Authorize]
        public ActionResult GetNewMessagesCount()
        {
            var receiverId = this._helper.GetUserEntity(User, _auth).Id;
            var result = this._rep.GetAllByReceiverId(receiverId).Where(m => !m.Read).Count();
            return Ok(result);
        }

        [HttpGet]
        [Route("getById/{id}")]
        [Authorize]
        public ActionResult GetByMessageId(int id)
        {
            var message = this._rep.GetById(id);
            if (_helper.GetUserEntity(User, _auth).Id != message.SenderId && _helper.GetUserEntity(User, _auth).Id != message.ReceiverId)
                return Unauthorized();

            var m = new
            {
                message.Id,
                message.MessageId,
                Receivers = _rep.GetReceivers(message.MessageId).Select(r => r.Name),
                Sender = _rep.GetSender(message.MessageId).Name,
                message.Read,
                message.Subject,
                message.Body,
                message.CreateDate,
                message.CreateTime,
            };

            return Ok(m);
        }

        [HttpPost]
        [Route("send")]
        [Authorize]
        public ActionResult Send([FromBody] SendMessageVM m)
        {
            if (this._helper.GetUserEntity(User, _auth).Id != m.SenderId)
                return Unauthorized();

            foreach(var r in m.ReceiverIds)
            {
                if (!this._helper.OwnesUser(User, r, _admin, _auth))
                    return Unauthorized();
            }

            var messageId = Guid.NewGuid().ToString();

            var datetime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(_helper.GetTimeZone(User, _auth, _restaurant, _city)));

            foreach (var r in m.ReceiverIds)
            {
                var msg = new Message()
                {
                    Body = m.Body,
                    MessageId = messageId,
                    CreateDate = datetime,
                    CreateTime = datetime.Hour.ToString() + ":" + datetime.Minute.ToString(),
                    Read = false,
                    ReceiverId = r,
                    ReplyToId = m.ReplyToMessageId ?? null,
                    SenderId = m.SenderId,
                    Subject = m.Subject
                };
                this._rep.Create(msg);
            }
            
            return Ok(messageId);
        }

        [HttpPut]
        [Route("markAsRead/{id}")]
        [Authorize]
        public ActionResult MarkAsRead(int id)
        {
            var m = _rep.GetById(id);
            if (_helper.GetUserEntity(User, _auth).Id != m.ReceiverId)
                return Unauthorized();

            this._rep.MarkAsRead(id);
            return Ok();
        }

        [HttpPut]
        [Route("markAsUnread/{id}")]
        [Authorize]
        public ActionResult MarkAsUnread(int id)
        {
            var m = _rep.GetById(id);
            if (_helper.GetUserEntity(User, _auth).Id != m.ReceiverId)
                return Unauthorized();

            this._rep.MarkAsUnread(id);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            var m = _rep.GetById(id);
            if (_helper.GetUserEntity(User, _auth).Id != m.ReceiverId)
                return Unauthorized();
            this._rep.Delete(id);
            return Ok();
        }

    }

    public class SendMessageVM
    {
        public string SenderId { get; set; }
        public string[] ReceiverIds { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int? ReplyToMessageId { get; set; }
    }


}
