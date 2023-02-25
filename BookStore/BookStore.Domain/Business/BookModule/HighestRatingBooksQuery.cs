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
    public class HighestRatingBooksQuery : IRequest<List<Book>>
    {

        public int Size { get; set; }


        public class HighestRatingBooksQueryHandler : IRequestHandler<HighestRatingBooksQuery, List<Book>>
        {
            private readonly BookStoreDbContext db;

            public HighestRatingBooksQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Book>> Handle(HighestRatingBooksQuery request, CancellationToken cancellationToken)
            {
                var books = await db.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Where(b => b.DeletedDate == null)
                    .OrderByDescending(b => b.Rate)
                    .Take(request.Size < 2 ? 2 : request.Size)
                    .ToListAsync(cancellationToken);

                return books;
            }
        }
    }
}
