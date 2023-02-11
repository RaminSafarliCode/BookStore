using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class WishlistQuery : Application.AppCode.Infrastructure.PaginateModel, IRequest<IEnumerable<Book>>
    {
        public class WishlistQueryHandler : IRequestHandler<WishlistQuery, IEnumerable<Book>>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public WishlistQueryHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<IEnumerable<Book>> Handle(WishlistQuery request, CancellationToken cancellationToken)
            {
                var favorites = ctx.ActionContext.HttpContext.Request.Cookies["favorites"]?
                                   .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                   .Where(x => Regex.IsMatch(x, @"\d+"))
                                   .Select(x => int.Parse(x))
                                   .ToArray();

                if (favorites == null || favorites.Length == 0)
                {
                    return Enumerable.Empty<Book>();
                }

                var favs = await db.Books
                    .Where(p => favorites.Contains(p.Id) && p.DeletedUserId == null)
                    .ToListAsync(cancellationToken);

                return favs;
            }
        }
    }
}
