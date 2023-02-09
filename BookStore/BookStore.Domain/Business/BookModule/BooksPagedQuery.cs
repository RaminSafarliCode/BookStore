using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BooksPagedQuery : PaginateModel, IRequest<PagedViewModel<Book>>
    {
        public class BooksPagedQueryHandler : IRequestHandler<BooksPagedQuery, PagedViewModel<Book>>
        {
            private readonly BookStoreDbContext db;

            public BooksPagedQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<PagedViewModel<Book>> Handle(BooksPagedQuery request, CancellationToken cancellationToken)
            {
                if (request.PageSize < 12)
                {
                    request.PageSize = 12;
                }
                var query = db.Books
                    .Include(p => p.Author)
                    .Include(p => p.Publisher)
                    .Include(p => p.Language)
                    .Include(p => p.Category)
                    .Where(m => m.DeletedDate == null)
                    .OrderByDescending(p => p.Id)
                    .AsQueryable();

                var pagedDate = new PagedViewModel<Book>(query, request);

                return pagedDate;
            }
        }
    }
}
