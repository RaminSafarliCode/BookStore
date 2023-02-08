using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.AuthorModule
{
    public class GetSingleAuthorQuery : IRequest<Author>
    {
        public int Id { get; set; }

        public class GetSingleAuthorQueryHandler : IRequestHandler<GetSingleAuthorQuery, Author>
        {
            private readonly BookStoreDbContext db;

            public GetSingleAuthorQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<Author> Handle(GetSingleAuthorQuery request, CancellationToken cancellationToken)
            {
                var author = await db.Authors
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return author;
            }
        }
    }

}
