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
    public class BlogPostGetAllQuery : PaginateModel, IRequest<PagedViewModel<BlogPost>>
    {
        public class BlogPostGetAllQueryHandler : IRequestHandler<BlogPostGetAllQuery, PagedViewModel<BlogPost>>
        {
            private readonly BookStoreDbContext db;

            public BlogPostGetAllQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<BlogPost>> Handle(BlogPostGetAllQuery request, CancellationToken cancellationToken)
            {
                if (request.PageSize < 6)
                {
                    request.PageSize = 6;
                }

                var query = db.BlogPosts
                        .Include(bp => bp.BlogPostCategory)
                       .Where(bp => bp.DeletedDate == null /*&& bp.PublishedDate != null*/)
                       .OrderByDescending(bp => bp.PublishedDate)
                       .AsQueryable();


                var pagedModel = new PagedViewModel<BlogPost>(query, request);


                return pagedModel;
            }
        }
    }
}
