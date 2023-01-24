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
    public class BlogPostCommentEntityTypeConfiguration : IEntityTypeConfiguration<BlogPostComment>
    {
        public void Configure(EntityTypeBuilder<BlogPostComment> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id)
                .UseIdentityColumn();

            builder.Property(entity => entity.Text)
                .IsRequired();

            builder.Property(entity => entity.BlogPostId)
                .IsRequired();
        }
    }
}
