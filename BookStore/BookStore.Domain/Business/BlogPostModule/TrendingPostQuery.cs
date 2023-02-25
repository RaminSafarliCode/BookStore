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

namespace BookStore.Domain.Business.BlogPostModule
{
    public class TrendingPostQuery : IRequest<List<BlogPost>>
    {

        public int Size { get; set; }

        public class TrendingPostQueryHandler : IRequestHandler<TrendingPostQuery, List<BlogPost>>
        {
            private readonly BookStoreDbContext db;

            public TrendingPostQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<List<BlogPost>> Handle(TrendingPostQuery request, CancellationToken cancellationToken)
            {
                var posts = await db.BlogPosts
                    .Include(bp => bp.BlogPostCategory)
                    .Include(bp => bp.Comments)
                    .Include(bp=>bp.CreatedByUser)
                    .Include(bp=>bp.Reacts)
                     .Where(bp => bp.DeletedDate == null && bp.PublishedDate != null)
                     .OrderByDescending(bp => bp.Reacts.Count())
                     .Take(request.Size < 2 ? 2 : request.Size)
                     .ToListAsync(cancellationToken);

                return posts;
            }
        }

    }
}
