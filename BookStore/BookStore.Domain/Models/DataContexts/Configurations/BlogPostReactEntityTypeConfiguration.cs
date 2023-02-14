using BookStore.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.DataContexts.Configurations
{
    public class BlogPostReactEntityTypeConfiguration : IEntityTypeConfiguration<BlogPostReact>
    {
        public void Configure(EntityTypeBuilder<BlogPostReact> builder)
        {
            builder.HasKey(key =>
            new
            {
                key.BlogPostId,
                key.CreatedByUserId
            });

            builder.ToTable("BlogPostRates");
        }
    }
}
