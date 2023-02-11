using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BookBasketQuery : IRequest<IEnumerable<Basket>>
    {
        public class BookBasketQueryHandler : IRequestHandler<BookBasketQuery, IEnumerable<Basket>>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public BookBasketQueryHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<IEnumerable<Basket>> Handle(BookBasketQuery request, CancellationToken cancellationToken)
            {
                if (!ctx.ActionContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    return Enumerable.Empty<Basket>();
                }

                var userId = ctx.GetCurrentUserId();

                var data = await db.Basket
                    .Include(b => b.Book)

                    .Where(b => b.UserId == userId)
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
