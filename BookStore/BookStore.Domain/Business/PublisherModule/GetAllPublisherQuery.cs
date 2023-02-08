using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.PublisherModule
{
    public class GetAllPublisherQuery : IRequest<List<Publisher>>
    {
        public class GetAllPublisherQueryHandler : IRequestHandler<GetAllPublisherQuery, List<Publisher>>
        {
            private readonly BookStoreDbContext db;

            public GetAllPublisherQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Publisher>> Handle(GetAllPublisherQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Publishers
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
