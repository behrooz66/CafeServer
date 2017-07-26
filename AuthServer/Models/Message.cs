using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace AuthServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; } //comma separated list of receiver IDs
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Read { get; set; }

        public int? ReplyToId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateTime { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }

        [ForeignKey("ReplyToId")]
        public virtual Message ReplyToMessage { get; set; }
    }
}
