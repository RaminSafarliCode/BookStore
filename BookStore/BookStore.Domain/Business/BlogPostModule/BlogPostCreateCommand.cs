using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BlogPostModule
{
    public class BlogPostCreateCommand : IRequest<JsonResponse>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImagePath { get; set; }
        public int BlogPostCategoryId { get; set; }
        public IFormFile Image { get; set; }
        public int[] TagIds { get; set; }

        public class BlogPostCreateCommandHandler : IRequestHandler<BlogPostCreateCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;
            private readonly IConfiguration configuration;
            private readonly IActionContextAccessor ctx;

            public BlogPostCreateCommandHandler(BookStoreDbContext db, IHostEnvironment env
                , IConfiguration configuration
                , IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.configuration = configuration;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(BlogPostCreateCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                    return null;

                var entity = new BlogPost();
                entity.TagCloud = new List<BlogPostTagItem>();

                entity.Title = request.Title;
                entity.Body = request.Body;
                entity.BlogPostCategoryId = request.BlogPostCategoryId;

                string extension = Path.GetExtension(request.Image.FileName);//.jpg

                request.ImagePath = $"blogpost-{Guid.NewGuid().ToString().ToLower()}{extension}";

                var folder = configuration["uploads:folder"];

                string fullPath = null;

                if (!string.IsNullOrWhiteSpace(folder))
                {
                    fullPath = folder.GetImagePhysicalPath(request.ImagePath);
                }
                else
                {
                    fullPath = env.GetImagePhysicalPath(request.ImagePath);
                }

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                entity.ImagePath = request.ImagePath;

                entity.Slug = request.Title.ToSlug();

                if (request.TagIds != null)
                {
                    foreach (var exceptedId in request.TagIds)
                    {
                        var tagItem = new BlogPostTagItem();
                        tagItem.TagId = exceptedId;
                        entity.TagCloud.Add(tagItem);
                    }
                }

                await db.BlogPosts.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                //return entity;
                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }
        }
    }
}
