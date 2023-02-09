using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.LanguageModule
{
    public class GetSingleLanguageQuery : IRequest<Language>
    {
        public int Id { get; set; }

        public class GetSingleLanguageQueryHandler : IRequestHandler<GetSingleLanguageQuery, Language>
        {
            private readonly BookStoreDbContext db;

            public GetSingleLanguageQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<Language> Handle(GetSingleLanguageQuery request, CancellationToken cancellationToken)
            {
                var author = await db.Languages
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return author;
            }
        }
    }

}
