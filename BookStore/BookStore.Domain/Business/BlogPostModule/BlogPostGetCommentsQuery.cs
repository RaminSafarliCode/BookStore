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
    public class BlogPostGetCommentsQuery : IRequest<List<BlogPostComment>>
    {
        public int BlogPostId { get; set; }
        public class BlogPostGetCommentsQueryHandler : IRequestHandler<BlogPostGetCommentsQuery, List<BlogPostComment>>
        {
            private readonly BookStoreDbContext db;

            public BlogPostGetCommentsQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<List<BlogPostComment>> Handle(BlogPostGetCommentsQuery request, CancellationToken cancellationToken)
            {
                var data = await db.BlogPostComments
                    .Include(bpc => bpc.BlogPost)
                    .Include(bpc => bpc.CreatedByUser)
                    .Where(bpc => bpc.DeletedDate == null && bpc.BlogPostId == request.BlogPostId)
                    .ToListAsync();

                return data;
            }
        }
    }
}
