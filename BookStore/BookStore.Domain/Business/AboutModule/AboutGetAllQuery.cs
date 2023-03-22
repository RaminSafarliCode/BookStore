using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.AboutModule
{
    public class AboutGetAllQuery : IRequest<List<About>>
    {
        public class AboutGetAllQueryHandler : IRequestHandler<AboutGetAllQuery, List<About>>
        {
            private readonly BookStoreDbContext db;

            public AboutGetAllQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<List<About>> Handle(AboutGetAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.AboutCompany
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
