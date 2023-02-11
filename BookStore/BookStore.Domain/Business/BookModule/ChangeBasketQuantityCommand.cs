using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Infrastructure;
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
    public class ChangeBasketQuantityCommand : IRequest<JsonResponse>
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }

        public class ChangeBasketQuantityCommandHandler : IRequestHandler<ChangeBasketQuantityCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public ChangeBasketQuantityCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }


            public async Task<JsonResponse> Handle(ChangeBasketQuantityCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var basketItem = await db.Basket.FirstOrDefaultAsync(b => b.BookId == request.BookId
                                            && b.UserId == userId, cancellationToken);

                if (basketItem == null)
                {
                    basketItem = new Basket();
                    basketItem.UserId = userId;
                    basketItem.BookId = request.BookId;
                    basketItem.Quantity = request.Quantity;

                    await db.Basket.AddAsync(basketItem, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);

                    var response = new JsonResponse
                    {
                        Error = false,
                        Message = "Sebete elave olundu"
                    };


                    var product = await db.Books.FirstOrDefaultAsync(b => b.Id == request.BookId
                                            && b.DeletedUserId == null, cancellationToken);

                    response.Value = new
                    {
                        Name = product.Name,
                        Price = product.Price,
                        Total = basketItem.Quantity * product.Price,
                        Summary = await db.Basket.Include(b => b.Book).SumAsync(b => b.Quantity * b.Book.Price, cancellationToken)
                    };


                    return response;
                }


                basketItem.Quantity = request.Quantity;

                await db.SaveChangesAsync(cancellationToken);


                var response2 = new JsonResponse
                {
                    Error = false,
                    Message = "Say deyishdirildi"
                };

                var product2 = await db.Books.FirstOrDefaultAsync(b => b.Id == request.BookId
                                            && b.DeletedUserId == null, cancellationToken);
                response2.Value = new
                {
                    Name = product2.Name,
                    Price = product2.Price,
                    Total = basketItem.Quantity * product2.Price,
                    Summary = await db.Basket.Include(b => b.Book).SumAsync(b => b.Quantity * b.Book.Price, cancellationToken)
                };

                return response2;
            }
        }
    }
}
