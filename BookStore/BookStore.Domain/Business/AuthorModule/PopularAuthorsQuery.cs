using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class PopularAuthorsQuery : IRequest<List<Author>>
    {

        public int Size { get; set; }


        public class PopularAuthorsQueryHandler : IRequestHandler<PopularAuthorsQuery, List<Author>>
        {
            private readonly BookStoreDbContext db;

            public PopularAuthorsQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Author>> Handle(PopularAuthorsQuery request, CancellationToken cancellationToken)
            {
                var authorOrders = await db.Orders
    .SelectMany(o => o.OrderBooks.Select(ob => new { ob.Book.Author, o }))
    .ToListAsync(cancellationToken);

                var authorGroupedOrders = authorOrders
                    .GroupBy(x => x.Author)
                    .Select(g => new
                    {
                        Author = g.Key,
                        NumOrders = g.Select(x => x.o.Id).Distinct().Count()
                    })
                    .OrderByDescending(x => x.NumOrders)
                    .ToList();

                var result = authorGroupedOrders.Select(x => x.Author).ToList();
                return result;


            }
        }
    }
}
