using MediatR;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace BookStore.Domain.Business.OrderModule
{
    public class OrderGetAllDeliveredQuery : IRequest<List<Order>>
    {
        public class OrderGetAllDeliveredHandler : IRequestHandler<OrderGetAllDeliveredQuery, List<Order>>
        {
            private readonly BookStoreDbContext db;

            public OrderGetAllDeliveredHandler(BookStoreDbContext db)
            {
                this.db = db;
            }


            public async Task<List<Order>> Handle(OrderGetAllDeliveredQuery request, CancellationToken cancellationToken)
            {
                var completedOrders = await db.Orders
                    .Where(o => o.DeletedDate == null && o.IsDelivered == true)
                    .ToListAsync(cancellationToken);


                return completedOrders;
            }
        }
    }
}
