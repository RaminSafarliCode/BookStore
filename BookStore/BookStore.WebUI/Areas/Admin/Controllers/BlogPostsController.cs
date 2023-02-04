using BookStore.Domain.Business.BlogPostModule;
using BookStore.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogPostsController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;
        public BlogPostsController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize("admin.blogposts.index")]
        public async Task<IActionResult> Index(BlogPostGetAllQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [Authorize(Policy = "admin.blogposts.details")]
        public async Task<IActionResult> Details(BlogPostSingleQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);  
        }

        [Authorize("admin.blogposts.create")]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.BlogPostCategories.Where(t => t.DeletedDate == null).ToList(), "Id", "Name");
            ViewBag.Tags = new SelectList(db.Tags.Where(t => t.DeletedDate == null).ToList(), "Id", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("admin.blogposts.create")]
        public async Task<IActionResult> Create(BlogPostCreateCommand command)
        {
            var response = await mediator.Send(command);

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(db.BlogPostCategories.ToList(), "Id", "Name", command.BlogPostCategoryId);
                ViewBag.Tags = new SelectList(db.Tags.Where(t => t.DeletedDate == null).ToList(), "Id", "Text");
                return View(command);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize("admin.blogposts.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await db.BlogPosts
                .Include(bp => bp.TagCloud)
                .FirstOrDefaultAsync(bp => bp.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(db.BlogPostCategories.ToList(), "Id", "Name", blogPost.BlogPostCategoryId);
            ViewBag.Tags = new SelectList(db.Tags.Where(t => t.DeletedDate == null).ToList(), "Id", "Text");

            
            var editCommand = new BlogPostEditCommand();
            editCommand.Id = blogPost.Id;
            editCommand.Title = blogPost.Title;
            editCommand.Body = blogPost.Body;
            editCommand.ImagePath = blogPost.ImagePath;
            editCommand.BlogPostCategoryId = blogPost.BlogPostCategoryId;
            editCommand.Id = blogPost.Id;
            editCommand.TagIds = blogPost.TagCloud.Select(tc => tc.TagId).ToArray();

            return View(editCommand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("admin.blogposts.edit")]
        public async Task<IActionResult> Edit(int id, BlogPostEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            if (response == null)
            {
                return NotFound();
            }
            if (response.Error == false)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize("admin.blogposts.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, BlogPostRemoveCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Publish")]
        [ValidateAntiForgeryToken]
        [Authorize("admin.blogposts.publish")]
        public async Task<IActionResult> PublishConfirmed(int id, BlogPostPublishCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [Authorize("admin.blogposts.deletedposts")]
        public async Task<IActionResult> DeletedPosts(BlogPostGetAllDeletedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        [HttpPost, ActionName("Back")]
        [ValidateAntiForgeryToken]
        [Authorize("admin.blogposts.removeback")]
        public async Task<IActionResult> BackToPosts(int id, BlogPostRemoveBackCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }

        [Authorize("admin.blogposts.deletedpostdetails")]
        public async Task<IActionResult> DeletedPostDetails(BlogPostSingleQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost, ActionName("Clear")]
        [ValidateAntiForgeryToken]
        [Authorize("admin.blogposts.cleardeletedposts")]
        public async Task<IActionResult> ClearDeletedPosts(int id, BlogPostClearCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        [Authorize("admin.blogposts.getcomments")]
        public async Task<IActionResult> GetComments(BlogPostGetCommentsQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize("admin.blogposts.commentdetails")]
        public async Task<IActionResult> CommentDetails(BlogPostGetSingleCommentQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize("admin.blogposts.commentdelete")]
        [HttpPost, ActionName("CommentDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentDeleteConfirmed(int id, BlogPostCommentRemoveCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response == null)
            {
                return NotFound();
            }


            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return db.BlogPosts.Any(e => e.Id == id);
        }
    }
}
