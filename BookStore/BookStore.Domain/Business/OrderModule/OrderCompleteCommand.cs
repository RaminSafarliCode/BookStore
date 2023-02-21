using MediatR;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.OrderModule
{
    public class OrderCompleteCommand : IRequest<Order>
    {
        public int Id { get; set; }

        public class OrderCompleteCommandHandler : IRequestHandler<OrderCompleteCommand, Order>
        {
            private readonly BookStoreDbContext db;

            public OrderCompleteCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Order> Handle(OrderCompleteCommand request, CancellationToken cancellationToken)
            {
                var data = await db.Orders.FirstOrDefaultAsync(o => o.Id == request.Id && o.DeletedDate == null);

                if (data == null)
                {
                    return null;
                }

                data.IsDelivered = true;
                await db.SaveChangesAsync(cancellationToken);

                return data;
            }
        }

    }
}
