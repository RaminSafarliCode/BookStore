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
    public class NewReleaseBookQuery : IRequest<List<Book>>
    {

        public int Size { get; set; }


        public class NewReleaseBookQueryHandler : IRequestHandler<NewReleaseBookQuery, List<Book>>
        {
            private readonly BookStoreDbContext db;

            public NewReleaseBookQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Book>> Handle(NewReleaseBookQuery request, CancellationToken cancellationToken)
            {
                var books = await db.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Where(b => b.DeletedDate == null)
                    .OrderByDescending(b => b.CreatedDate)
                    .Take(request.Size < 2 ? 2 : request.Size)
                    .ToListAsync(cancellationToken);

                return books;
            }
        }
    }
}
