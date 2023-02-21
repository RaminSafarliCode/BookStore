using MediatR;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.OrderModule
{
    public class OrderGetSingleQuery : IRequest<Order>
    {
        public int Id { get; set; }

        public class OrderGetSingleQueryHandler : IRequestHandler<OrderGetSingleQuery, Order>
        {
            private readonly BookStoreDbContext db;

            public OrderGetSingleQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Order> Handle(OrderGetSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Orders
                    //.Include(o => o.User)
                    .Include(o => o.OrderBooks.Where(op => op.DeletedDate == null))
                    .ThenInclude(op => op.Book)
                    //.ThenInclude(p => p.BookImages.Where(i => i.DeletedDate == null && i.IsMain == true))
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return data;
            }
        }

    }
}
