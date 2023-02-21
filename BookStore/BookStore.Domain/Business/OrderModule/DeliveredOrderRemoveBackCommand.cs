using MediatR;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.OrderModule
{
    public class DeliveredOrderRemoveBackCommand : IRequest<Order>
    {
        public int Id { get; set; }
        public class DeliveredOrderRemoveBackCommandHandler : IRequestHandler<DeliveredOrderRemoveBackCommand, Order>
        {
            private readonly BookStoreDbContext db;

            public DeliveredOrderRemoveBackCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Order> Handle(DeliveredOrderRemoveBackCommand request, CancellationToken cancellationToken)
            {
                var data = db.Orders.FirstOrDefault(m => m.Id == request.Id && m.IsDelivered == true);

                if (data == null)
                {
                    return null;
                }

                data.IsDelivered = false;

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}

