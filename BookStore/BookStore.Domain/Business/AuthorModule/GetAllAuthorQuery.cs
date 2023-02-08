using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.AuthorModule
{
    public class GetAllAuthorQuery : IRequest<List<Author>>
    {
        public class GetAllAuthorQueryHandler : IRequestHandler<GetAllAuthorQuery, List<Author>>
        {
            private readonly BookStoreDbContext db;

            public GetAllAuthorQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Author>> Handle(GetAllAuthorQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Authors
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
