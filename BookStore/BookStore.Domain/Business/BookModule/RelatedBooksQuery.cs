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
    public class RelatedBooksQuery : IRequest<List<Book>>
    {

        //public int Size { get; set; }
        //public int Id { get; set; }
        public string Category { get; set; }


        public class RelatedBooksQueryHandler : IRequestHandler<RelatedBooksQuery, List<Book>>
        {
            private readonly BookStoreDbContext db;

            public RelatedBooksQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Book>> Handle(RelatedBooksQuery request, CancellationToken cancellationToken)
            {

                var books = await db.Books
                    .Include(b => b.Category)
                    .Include(b => b.Author)
                    .Include(b => b.Orders) 
                    .Where(b => b.DeletedDate == null && b.Category.Name.Equals(request.Category))
                    //.Take(request.Size < 2 ? 2 : request.Size)
                    .ToListAsync(cancellationToken);

                return books;
            }
        }
    }
}
