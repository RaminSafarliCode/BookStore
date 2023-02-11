using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
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
    public class RemoveFromBasketCommand : IRequest<JsonResponse>
    {
        public int BookId { get; set; }

        public class RemoveFromBasketCommandHandler : IRequestHandler<RemoveFromBasketCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public RemoveFromBasketCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }


            public async Task<JsonResponse> Handle(RemoveFromBasketCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var basketItem = await db.Basket.FirstOrDefaultAsync(b => b.BookId == request.BookId
                                            && b.UserId == userId, cancellationToken);

                if (basketItem == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Sebetde movcud deyil"
                    };
                }

                db.Basket.Remove(basketItem);
                await db.SaveChangesAsync(cancellationToken);

                var info = await (from b in db.Basket
                                  join p in db.Books on b.BookId equals p.Id
                                  where b.UserId == userId
                                  select new
                                  {
                                      b.UserId,
                                      SubTotal = p.Price * b.Quantity
                                  })
                                  .GroupBy(g => g.UserId)
                                  .Select(g => new
                                  {
                                      Total = g.Sum(m => m.SubTotal),
                                      Count = g.Count()
                                  })
                                  .FirstOrDefaultAsync(cancellationToken);

                return new JsonResponse
                {
                    Error = false,
                    Message = "Sebetden silindi",
                    Value = info
                };
            }
        }
    }
}
