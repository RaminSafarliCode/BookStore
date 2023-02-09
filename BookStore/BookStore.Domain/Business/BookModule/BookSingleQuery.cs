using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BookSingleQuery : IRequest<Book>
    {
        public int Id { get; set; }

        public class BookSingleQueryHandler : IRequestHandler<BookSingleQuery, Book>
        {
            private readonly BookStoreDbContext db;

            public BookSingleQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<Book> Handle(BookSingleQuery request, CancellationToken cancellationToken)
            {
                var book = await db.Books
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return book;
            }
        }
    }
}
