using BookStore.Domain.Models.Entities.Infrastructure;
using BookStore.Domain.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities.Chat
{
    public class Message : BaseEntity
    {
        public int? ToId { get; set; }
        public virtual BookStoreUser To { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
        public string Text { get; set; }
    }
}
