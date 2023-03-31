using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Migrations;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class AddToBasketCommand : IRequest<JsonResponse>
    {
        public int BookId { get; set; }

        public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public AddToBasketCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }


            public async Task<JsonResponse> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var alreadyExists = await db.Basket.AnyAsync(b => b.BookId == request.BookId && b.UserId == userId, cancellationToken);

                if (alreadyExists)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Already added to basket"
                    };
                }


                var basketItem = new Basket
                {
                    UserId = userId,
                    BookId = request.BookId,
                    Quantity = 1
                };

                await db.Basket.AddAsync(basketItem, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var value = ctx.GetIntArrayFromCookie("favorites");

                if (value != null)
                {
                    ctx.SetValueToCookie("favorites", value.Where(e => e != request.BookId).ToArray());
                }

                var info = await (from bas in db.Basket
                                  join b in db.Books on bas.BookId equals b.Id
                                  where bas.UserId == userId
                                  select new
                                  {
                                      bas.UserId, 
                                      SubTotal = b.Price * bas.Quantity
                                  }).GroupBy(g => g.UserId)
                                  .Select(g => new
                                  {
                                      Total = g.Sum(m => m.SubTotal),
                                     Count = g.Count()
                                  }).FirstOrDefaultAsync(cancellationToken);


                return new JsonResponse
                {
                    Error = false,
                    Message = "Added to cart",
                    Value = new
                    {
                        Book = basketItem,
                        BasketInfo = info
                    }
                };
            }
        }
    }
}
