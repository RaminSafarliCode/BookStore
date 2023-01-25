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
    public class BlogPostSingleQuery : IRequest<BlogPost>
    {
        public int Id { get; set; }
        public string Slug { get; set; }

        public class BlogPostSingleQueryHandler : IRequestHandler<BlogPostSingleQuery, BlogPost>
        {
            private readonly BookStoreDbContext db;

            public BlogPostSingleQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<BlogPost> Handle(BlogPostSingleQuery request, CancellationToken cancellationToken)
            {
                var query = db.BlogPosts
                            .Include(bp => bp.BlogPostCategory)
                            .Include(bp => bp.Comments)
                            .ThenInclude(c => c.CreatedByUser)

                            .Include(bp => bp.TagCloud)
                            .ThenInclude(bp => bp.Tag)
                            .AsQueryable();

                if (string.IsNullOrWhiteSpace(request.Slug))
                {
                    return await query.FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null);
                }

                return await query.FirstOrDefaultAsync(m => m.Slug.Equals(request.Slug) && m.DeletedDate == null);
            }
        }
    }
}
