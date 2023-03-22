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
    public class SearchBookQuery : IRequest<List<Book>>
    {
        public string SearchText { get; set; }

        public class SearchBookQueryHandler : IRequestHandler<SearchBookQuery, List<Book>>
        {
            private readonly BookStoreDbContext db;

            public SearchBookQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Book>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
            {
                var query = db.Books
                    .Include(b=>b.Author)
                    .Include(b=>b.Category)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    query = query.Where(b => b.Name.Contains(request.SearchText) || b.Author.Name.Contains(request.SearchText));
                }

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
