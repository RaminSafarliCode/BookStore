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
    public class BookFilterQuery : PaginateModel, IRequest<PagedViewModel<Book>>
    {
        public int [] Authors { get; set; }
        public int [] Publishers { get; set; }
        public int [] Languages { get; set; }
        public int [] Categories { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }

        public class BookFilterQueryHandler : IRequestHandler<BookFilterQuery, PagedViewModel<Book>>
        {
            private readonly BookStoreDbContext db;

            public BookFilterQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<PagedViewModel<Book>> Handle(BookFilterQuery request, CancellationToken cancellationToken)
            {
                if (request.PageSize < 12)
                {
                    request.PageSize = 12;
                }

                var query = db.Books.AsQueryable();

                if (request.Authors != null && request.Authors.Length > 0)
                {
                    query = query.Where(q => request.Authors.Contains(q.AuthorId));
                }

                if (request.Languages != null && request.Languages.Length > 0)
                {
                    query = query.Where(q => request.Languages.Contains(q.LanguageId));
                }

                if (request.Publishers != null && request.Publishers.Length > 0)
                {
                    query = query.Where(q => request.Publishers.Contains(q.PublisherId));
                }

                if (request.Categories != null && request.Categories.Length > 0)
                {
                    query = query.Where(q => request.Categories.Contains(q.CategoryId));
                }

                var productIds = await query.Select(q => q.Id)
                    .Distinct()
                    .ToArrayAsync(cancellationToken);

                var productQuery = db.Books
                    .Where(p => productIds.Contains(p.Id))
                    .AsQueryable();

                if (request.Min > 0 && request.Min <= request.Max)
                {
                    productQuery = productQuery.Where(q => q.Price >= request.Min && q.Price <= request.Max);
                }

                productQuery = productQuery
                    .Include(p => p.Author)
                    .Include(p => p.Language)
                    .Include(p => p.Publisher)
                    .Include(p => p.Category)

                    .OrderByDescending(q => q.Id);

                var pagedModel = new PagedViewModel<Book>(productQuery, request);

                return pagedModel;
            }
        }
    }
}
