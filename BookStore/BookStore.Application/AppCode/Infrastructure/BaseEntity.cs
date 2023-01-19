using System;

namespace BookStore.Application.AppCode.Infrastructure
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
        public int? DeletedUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
