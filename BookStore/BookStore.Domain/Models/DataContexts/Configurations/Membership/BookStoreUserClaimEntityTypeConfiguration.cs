using BookStore.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Domain.Models.DataContexts.Configurations.Membership
{
    public class BookStoreUserClaimEntityTypeConfiguration : IEntityTypeConfiguration<BookStoreUserClaim>
    {
        public void Configure(EntityTypeBuilder<BookStoreUserClaim> builder)
        {
            builder.ToTable("UserClaims", "Membership");
        }
    }
}
