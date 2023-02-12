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
    public class SetRateCommand : IRequest<JsonResponse>
    {
        public int BookId { get; set; }
        public byte Rate { get; set; }

        public class SetRateCommandHandler : IRequestHandler<SetRateCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public SetRateCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(SetRateCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var rateEntry = await db.BookRates.FirstOrDefaultAsync(m => m.BookId == request.BookId && m.CreatedByUserId == userId, cancellationToken);

                if (rateEntry != null)
                {
                    rateEntry.Rate = request.Rate;
                    rateEntry.CreatedDate = DateTime.UtcNow.AddHours(4);
                    await db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    rateEntry = new BookRate
                    {
                        BookId = request.BookId,
                        Rate = request.Rate,
                        CreatedByUserId = userId,
                        CreatedDate = DateTime.UtcNow.AddHours(4)
                    };
                    await db.BookRates.AddAsync(rateEntry, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);
                };

                var avgRate = db.BookRates.Where(m => m.BookId == request.BookId).Average(m => m.Rate);
                var book = await db.Books.FirstOrDefaultAsync(m => m.Id == request.BookId, cancellationToken);

                book.Rate = avgRate;
                await db.SaveChangesAsync(cancellationToken);

                return new JsonResponse
                {
                    Error = false,
                    Message = "Okay",
                    Value = avgRate
                };
            }
        }
    }
}
