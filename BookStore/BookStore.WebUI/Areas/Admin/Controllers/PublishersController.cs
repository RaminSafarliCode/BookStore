using BookStore.Domain.Business.PublisherModule;
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
    public class PublishersController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public PublishersController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.publishers.index")]
        public async Task<IActionResult> Index(GetAllPublisherQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.publishers.details")]
        public async Task<IActionResult> Details(GetSinglePublisherQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.publishers.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.publishers.create")]
        public async Task<IActionResult> Create(CreatePublisherCommand command)
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

        [Authorize(Policy = "admin.publishers.edit")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var publisher = await db.Publishers
                .FirstOrDefaultAsync(bp => bp.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }


            var editCommand = new EditPublisherCommand();
            editCommand.Id = publisher.Id;
            editCommand.Name = publisher.Name;

            return View(editCommand);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.publishers.edit")]
        public async Task<IActionResult> Edit(int id, EditPublisherCommand command)
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
        [Authorize(Policy = "admin.publishers.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, RemovePublisherCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return db.Publishers.Any(e => e.Id == id);
        }
    }
}
