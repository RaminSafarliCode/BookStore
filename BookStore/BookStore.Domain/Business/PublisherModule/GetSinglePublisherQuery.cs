using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.PublisherModule
{
    public class GetSinglePublisherQuery : IRequest<Publisher>
    {
        public int Id { get; set; }

        public class GetSinglePublisherQueryHandler : IRequestHandler<GetSinglePublisherQuery, Publisher>
        {
            private readonly BookStoreDbContext db;

            public GetSinglePublisherQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<Publisher> Handle(GetSinglePublisherQuery request, CancellationToken cancellationToken)
            {
                var author = await db.Publishers
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return author;
            }
        }
    }

}
