using BookStore.Domain.Models.Entities.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities.Infrastructure
{
    public class BaseEntity : AutitableEntity
    {
        public int Id { get; set; }
    }

    public class AutitableEntity
    {
        public int? CreatedByUserId { get; set; }
        public BookStoreUser CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int? DeletedUserId { get; set; }
        public BookStoreUser DeletedByUser { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
