using BookStore.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Domain.Models.DataContexts.Configurations.Membership
{
    public class BookStoreUserEntityTypeConfiguration : IEntityTypeConfiguration<BookStoreUser>
    {
        public void Configure(EntityTypeBuilder<BookStoreUser> builder)
        {
            builder.ToTable("Users", "Membership");
        }
    }
}
