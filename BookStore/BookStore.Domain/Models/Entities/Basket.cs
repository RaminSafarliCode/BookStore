using BookStore.Domain.Models.Entities.Membership;

namespace BookStore.Domain.Models.Entities
{
    public class Basket
    {
        public int UserId { get; set; }
        public virtual BookStoreUser User { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
