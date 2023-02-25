using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BestsellerBooksQuery : IRequest<List<Book>>
    {

        public int Size { get; set; }


        public class BestsellerBooksQueryHandler : IRequestHandler<BestsellerBooksQuery, List<Book>>
        {
            private readonly BookStoreDbContext db;

            public BestsellerBooksQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Book>> Handle(BestsellerBooksQuery request, CancellationToken cancellationToken)
            {
                var books = await db.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Include(b => b.Orders)
                    .Where(b => b.DeletedDate == null)
                    .OrderByDescending(b => b.Orders.Count())
                    .Take(request.Size < 2 ? 2 : request.Size)
                    .ToListAsync(cancellationToken);

                return books;
            }
        }
    }
}
