using BookStore.Domain.Models.Entities;
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

        public DbSet<ContactPost> ContactPosts { get; set; } 
        public DbSet<Faq> Faqs { get; set; } 
        public DbSet<Subscribe> Subscribes { get; set; } 
        public DbSet<BlogPost> BlogPosts { get; set; } 
        public DbSet<Tag> Tags { get; set; } 
        public DbSet<BlogPostCategory> BlogPostCategories { get; set; } 
        public DbSet<BlogPostTagItem> BlogPostTagCloud { get; set; } 
        public DbSet<BlogPostComment> BlogPostComments { get; set; } 
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Author> Authors { get; set; } 
        public DbSet<Publisher> Publishers { get; set; } 
        public DbSet<Language> Languages { get; set; } 
        public DbSet<Book> Books{ get; set; }
        public DbSet<Basket> Basket { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);
        }
    }
}
