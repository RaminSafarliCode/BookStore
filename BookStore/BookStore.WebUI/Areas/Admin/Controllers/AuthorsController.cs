using BookStore.Domain.Business.AuthorModule;
using BookStore.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public AuthorsController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.authors.index")]
        public async Task<IActionResult> Index(GetAllAuthorQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.authors.details")]
        public async Task<IActionResult> Details(GetSingleAuthorQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.authors.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.authors.create")]
        public async Task<IActionResult> Create(CreateAuthorCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                if (response == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [Authorize(Policy = "admin.authors.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var entity = await db.Categories
            //    .FirstOrDefaultAsync(c => c.Id == id);


            //if (entity == null)
            //{
            //    return NotFound();
            //}


            //command.Id = entity.Id;
            //command.Name = entity.Name;

            //return View(command);

            if (id == null)
            {
                return NotFound();
            }

            var author = await db.Authors
                .FirstOrDefaultAsync(bp => bp.Id == id);
            if (author == null)
            {
                return NotFound();
            }


            var editCommand = new EditAuthorCommand();
            editCommand.Id = author.Id;
            editCommand.Name = author.Name;
            editCommand.Biography = author.Biography;
            editCommand.ImagePath = author.ImagePath;

            return View(editCommand);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.authors.edit")]
        public async Task<IActionResult> Edit(int id, EditAuthorCommand command)
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
        [Authorize(Policy = "admin.authors.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, RemoveAuthorCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return db.Authors.Any(e => e.Id == id);
        }
    }
}
