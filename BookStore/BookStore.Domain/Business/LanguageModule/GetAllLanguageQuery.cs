using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.LanguageModule
{
    public class GetAllLanguageQuery : IRequest<List<Language>>
    {
        public class GetAllLanguageQueryHandler : IRequestHandler<GetAllLanguageQuery, List<Language>>
        {
            private readonly BookStoreDbContext db;

            public GetAllLanguageQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Language>> Handle(GetAllLanguageQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Languages
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
