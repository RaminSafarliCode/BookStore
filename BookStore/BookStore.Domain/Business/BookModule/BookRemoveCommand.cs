using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BookRemoveCommand : IRequest<Book>
    {
        public int Id { get; set; }

        public class BookRemoveCommandHandler : IRequestHandler<BookRemoveCommand, Book>
        {
            private readonly BookStoreDbContext db;

            public BookRemoveCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Book> Handle(BookRemoveCommand request, CancellationToken cancellationToken)
            {
                var data = db.Books
                    .FirstOrDefault(m => m.Id == request.Id && m.DeletedDate == null);

                if (data == null)
                {
                    throw new Exception("Book not found!");
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}
