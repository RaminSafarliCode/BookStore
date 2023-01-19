using BookStore.Domain.Models.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Domain.Models.DataContexts.Configurations.Membership
{
    public class BookStoreUserTokenEntityTypeConfiguration : IEntityTypeConfiguration<BookStoreUserToken>
    {
        public void Configure(EntityTypeBuilder<BookStoreUserToken> builder)
        {
            builder.ToTable("UserTokens", "Membership");
        }
    }
}
