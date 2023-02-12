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
    public class BookRateEntityTypeConfiguration : IEntityTypeConfiguration<BookRate>
    {
        public void Configure(EntityTypeBuilder<BookRate> builder)
        {
            builder.HasKey(key =>
            new
            {
                key.BookId,
                key.CreatedByUserId
            });

            builder.ToTable("BookRates");
        }
    }
}
