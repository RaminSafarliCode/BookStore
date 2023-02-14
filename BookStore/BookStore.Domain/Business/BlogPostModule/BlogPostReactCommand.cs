using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BlogPostModule
{
    public class BlogPostReactCommand : IRequest<JsonResponse>
    {
        public int BlogPostId { get; set; }

        public int UserId { get; set; }

        public bool isLiked { get; set; }

        public class BlogPostReactCommandHandler : IRequestHandler<BlogPostReactCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;

            public BlogPostReactCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<JsonResponse> Handle(BlogPostReactCommand request, CancellationToken cancellationToken)
            {

                #region Check blog post and user

                var blogPost = await db.BlogPosts.FirstOrDefaultAsync(bp => bp.Id == request.BlogPostId, cancellationToken);

                if (blogPost == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Invalid request!"
                    };
                }


                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

                if (user == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Invalid request!"
                    };
                }

                #endregion


                if (request.isLiked)
                {
                    if (await db.BlogPostReacts.AnyAsync(bpl => bpl.BlogPostId == request.BlogPostId && bpl.CreatedByUserId == request.UserId))
                    {
                        return new JsonResponse
                        {
                            Error = false,
                            Message = "You have liked the post!"
                        };
                    }
                    else
                    {
                        db.BlogPostReacts.Add(new BlogPostReact
                        {
                            BlogPostId = request.BlogPostId,
                            CreatedByUserId = request.UserId
                        });
                    }


                    await db.SaveChangesAsync();

                    var likeCount = await db.BlogPostReacts.Where(bpl => bpl.CreatedByUserId == request.UserId && bpl.BlogPostId == request.BlogPostId).CountAsync();

                    return new JsonResponse
                    {
                        Error = false,
                        Message = "You liked the post!",
                        Value = likeCount
                    };
                }
                else
                {
                    var blogPostLike = await db.BlogPostReacts.FirstOrDefaultAsync(bpl => bpl.BlogPostId == request.BlogPostId && bpl.CreatedByUserId == request.UserId);

                    if (blogPostLike == null)
                    {
                        return new JsonResponse
                        {
                            Error = true,
                            Message = "You have not like the post!"
                        };
                    }
                    else
                    {
                        db.BlogPostReacts.Remove(blogPostLike);

                        await db.SaveChangesAsync();

                        var likeCount = await db.BlogPostReacts.Where(bpl => bpl.CreatedByUserId == request.UserId && bpl.BlogPostId == request.BlogPostId).CountAsync();

                        return new JsonResponse
                        {
                            Error = false,
                            Message = "You unliked the post!",
                            Value = likeCount
                        };
                    }

                }
            }
        }
    }
}
