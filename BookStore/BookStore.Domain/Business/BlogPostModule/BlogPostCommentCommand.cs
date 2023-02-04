using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BlogPostModule
{
    public class BlogPostCommentCommand : IRequest<BlogPostComment>
    {
        public int? CommentId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The Post ID is not valid!")]
        public int PostId { get; set; }

        [Required]
        public string Comment { get; set; }

        public class BlogPostCommentCommandHandler : IRequestHandler<BlogPostCommentCommand, BlogPostComment>
        {

            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public BlogPostCommentCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<BlogPostComment> Handle(BlogPostCommentCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.ActionContext.ModelState.IsValid)
                {

                    throw new Exception(ctx.ActionContext.ModelState.GetErrors().FirstOrDefault().Message);
                }

                var post = await db.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == request.PostId);

                if (post == null)
                {
                    throw new Exception("The post is not available!");
                }

                var commentModel = new BlogPostComment
                {
                    BlogPostId = request.PostId,
                    Text = request.Comment,
                    CreatedByUserId = ctx.GetCurrentUserId()
                };



                if (request.CommentId.HasValue && await db.BlogPostComments.AnyAsync(c => c.Id == request.CommentId))
                {
                    commentModel.ParentId = request.CommentId;
                }

                db.BlogPostComments.Add(commentModel);
                await db.SaveChangesAsync();

                commentModel = await db.BlogPostComments
                    .Include(c => c.CreatedByUser)
                    .Include(c => c.Parent)
                    .FirstOrDefaultAsync(c => c.Id == commentModel.Id);

                return commentModel;
            }
        }
    }
}
