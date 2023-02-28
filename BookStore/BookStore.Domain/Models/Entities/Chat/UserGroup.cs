using BookStore.Domain.Models.Entities.Membership;

namespace BookStore.Domain.Models.Entities.Chat
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public virtual BookStoreUser User { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

    }
}
