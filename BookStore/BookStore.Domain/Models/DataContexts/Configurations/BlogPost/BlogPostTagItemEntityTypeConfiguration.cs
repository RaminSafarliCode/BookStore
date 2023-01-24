using BookStore.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.DataContexts.Configurations.BlogPost
{
    public class BlogPostTagItemEntityTypeConfiguration : IEntityTypeConfiguration<BlogPostTagItem>
    {
        public void Configure(EntityTypeBuilder<BlogPostTagItem> builder)
        {
            builder.HasKey(entity => new { entity.BlogPostId, entity.TagId });

            builder.Property(entity => entity.Id)
                .UseIdentityColumn();

            builder.ToTable("BlogPostTagCloud");
        }
    }
}
