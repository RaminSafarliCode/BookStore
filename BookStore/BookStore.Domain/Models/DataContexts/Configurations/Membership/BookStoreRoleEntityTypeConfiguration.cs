using BookStore.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Domain.Models.DataContexts.Configurations.Membership
{
    public class BookStoreRoleEntityTypeConfiguration : IEntityTypeConfiguration<BookStoreRole>
    {
        public void Configure(EntityTypeBuilder<BookStoreRole> builder)
        {
            builder.ToTable("Roles", "Membership");
        }
    }
}
