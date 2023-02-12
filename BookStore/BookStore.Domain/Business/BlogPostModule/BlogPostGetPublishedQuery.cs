using BookStore.Application.AppCode.Infrastructure;
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
    public class BlogPostGetPublishedQuery : PaginateModel, IRequest<PagedViewModel<BlogPost>>
    {
        public class BlogPostGetPublishedQueryHandler : IRequestHandler<BlogPostGetPublishedQuery, PagedViewModel<BlogPost>>
        {
            private readonly BookStoreDbContext db;

            public BlogPostGetPublishedQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<BlogPost>> Handle(BlogPostGetPublishedQuery request, CancellationToken cancellationToken)
            {
                if (request.PageSize < 6)
                {
                    request.PageSize = 6;
                }

                var query = db.BlogPosts
                    
                       .Where(bp => bp.DeletedDate == null && bp.PublishedDate != null)

                       .Include(bp => bp.CreatedByUser)

                       .Include(bp=>bp.Comments)

                       .OrderByDescending(bp => bp.PublishedDate)
                       .AsQueryable();


                var pagedModel = new PagedViewModel<BlogPost>(query, request);


                return pagedModel;
            }
        }
    }
}
