using BookStore.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.DataContexts
{
    public class BookStoreDbContext : IdentityDbContext<BookStoreUser, BookStoreRole, int, BookStoreUserClaim, BookStoreUserRole, BookStoreUserLogin, BookStoreRoleClaim, BookStoreUserToken>
    {
        public BookStoreDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookStoreUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });
            builder.Entity<BookStoreRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            builder.Entity<BookStoreUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");

            });
            builder.Entity<BookStoreUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            builder.Entity<BookStoreRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");

            });
            builder.Entity<BookStoreUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });
            builder.Entity<BookStoreUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });
        }
    }
}
