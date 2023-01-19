using BookStore.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Domain.Models.DataContexts.Configurations.Membership
{
    public class BookStoreUserRoleEntityTypeConfiguration : IEntityTypeConfiguration<BookStoreUserRole>
    {
        public void Configure(EntityTypeBuilder<BookStoreUserRole> builder)
        {
            builder.ToTable("UserRoles", "Membership");
        }
    }
}
