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
    public class ContactPostEntityTypeConfiguration : IEntityTypeConfiguration<ContactPost>
    {
        public void Configure(EntityTypeBuilder<ContactPost> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .UseIdentityColumn();

            builder.Property(entity => entity.FirstName)
                .IsRequired();

            builder.Property(entity => entity.LastName)
                .IsRequired();

            builder.Property(entity => entity.Email)
                .IsRequired();

            builder.Property(entity => entity.Comment)
                .IsRequired();
        }
    }
}
