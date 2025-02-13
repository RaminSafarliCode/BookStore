﻿using BookStore.Domain.Models.Entities;
using BookStore.Domain.Models.Entities.Chat;
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
        public DbSet<BlogPostReact> BlogPostReacts { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Author> Authors { get; set; } 
        public DbSet<Publisher> Publishers { get; set; } 
        public DbSet<Language> Languages { get; set; } 
        public DbSet<Book> Books{ get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<BookRate> BookRates { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBook> OrderBooks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> ChatGroups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<BookStoreForgotPassword> BookStoreForgotPasswords { get; set; }
        public DbSet<About> AboutCompany { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);
        }
    }
}
