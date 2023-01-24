using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BlogPostModule
{
    public class BlogPostEditCommand : IRequest<BlogPost>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImagePath { get; set; }
        public int BlogPostCategoryId { get; set; }
        public IFormFile Image { get; set; }
        public int[] TagIds { get; set; }

        public class BlogPostEditCommandHandler : IRequestHandler<BlogPostEditCommand, BlogPost>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;
            private readonly IActionContextAccessor ctx;

            public BlogPostEditCommandHandler(BookStoreDbContext db, IHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<BlogPost> Handle(BlogPostEditCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                    return null;

                var entity = await db.BlogPosts
                                     .Include(bp => bp.TagCloud)
                                     .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.Title = request.Title;
                entity.Body = request.Body;
                entity.BlogPostCategoryId = request.BlogPostCategoryId;

                if (request.Image == null)
                    goto end;

                string extension = Path.GetExtension(request.Image.FileName);//.jpg

                request.ImagePath = $"blogpost-{Guid.NewGuid().ToString().ToLower()}{extension}";
                string fullPath = env.GetImagePhysicalPath(request.ImagePath);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                env.ArchiveImage(entity.ImagePath);

                entity.ImagePath = request.ImagePath;

            end:
                if (string.IsNullOrWhiteSpace(entity.Slug))
                {
                    entity.Slug = request.Title.ToSlug();
                }

                if (request.TagIds == null && entity.TagCloud.Any())
                {
                    foreach (var tagItem in entity.TagCloud)
                    {
                        db.BlogPostTagCloud.Remove(tagItem);
                    }
                }
                else if (request.TagIds != null)
                {
                    #region databasede evvel olan indi olmayan tagler-silinmesini istediklerimiz
                    // 1,2,3  => 1,3
                    var exceptedIds = db.BlogPostTagCloud.Where(tc => tc.BlogPostId == entity.Id).Select(tc => tc.TagId).ToList()
                        .Except(request.TagIds).ToArray();

                    if (exceptedIds.Length > 0)
                    {
                        foreach (var exceptedId in exceptedIds)
                        {
                            var tagItem = db.BlogPostTagCloud.FirstOrDefault(tc => tc.TagId == exceptedId
                            && tc.BlogPostId == entity.Id);

                            if (tagItem != null)
                            {
                                db.BlogPostTagCloud.Remove(tagItem);
                            }
                        }
                    }
                    #endregion

                    #region evvel databasede olmayan ama indi elave olunmasini istediklerimiz
                    var newExceptedIds = request.TagIds.Except(db.BlogPostTagCloud.Where(tc => tc.BlogPostId == entity.Id).Select(tc => tc.TagId).ToList()).ToArray();

                    if (newExceptedIds.Length > 0)
                    {
                        foreach (var exceptedId in newExceptedIds)
                        {
                            var tagItem = new BlogPostTagItem();
                            tagItem.TagId = exceptedId;
                            tagItem.BlogPostId = entity.Id;
                            await db.BlogPostTagCloud.AddAsync(tagItem);
                        }
                    }
                    #endregion
                }


                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
