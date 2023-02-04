using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Business.BlogPostModule;
using BookStore.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IMediator mediator;
        private readonly BookStoreDbContext db;

        public BlogController(IMediator mediator, BookStoreDbContext db)
        {
            this.mediator = mediator;
            this.db = db;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BlogPostGetPublishedQuery query)
        {
            var response = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Body", response);
            }

            return View(response);
        }

        [AllowAnonymous]
        [Route("/blog/{slug}")]
        public async Task<IActionResult> Details(BlogPostSingleQuery query, int id)
        {
            //var post = await db.BlogPosts.FirstOrDefaultAsync(entity => entity.Id == id);

            //if (post == null)
            //{
            //    return NotFound();
            //}

            //db.BlogPosts.


            var blogPost = await mediator.Send(query);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(BlogPostCommentCommand command)
        {

            try
            {
                var response = await mediator.Send(command);
                return PartialView("_AddedComment", response);
                //return Json(new
                //{
                //    error = false,
                //    message = "Successsssssss"
                //});
            }
            catch (System.Exception ex)
            {
                return Json(new
                {
                    error = true,
                    message = ex.Message
                });
            }
        }
    }
}
