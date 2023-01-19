using BookStore.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Domain.Models.DataContexts.Configurations.Membership
{
    public class BookStoreUserLoginEntityTypeConfiguration : IEntityTypeConfiguration<BookStoreUserLogin>
    {
        public void Configure(EntityTypeBuilder<BookStoreUserLogin> builder)
        {
            builder.ToTable("UserLogins", "Membership");
        }
    }
}
