using BookStore.Domain.Business.LanguageModule;
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
    public class LanguagesController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public LanguagesController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.languages.index")]
        public async Task<IActionResult> Index(GetAllLanguageQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.languages.details")]
        public async Task<IActionResult> Details(GetSingleLanguageQuery query)
        {
            var response = await mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [Authorize(Policy = "admin.languages.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.languages.create")]
        public async Task<IActionResult> Create(CreateLanguageCommand command)
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

        [Authorize(Policy = "admin.languages.edit")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var publisher = await db.Languages
                .FirstOrDefaultAsync(bp => bp.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }


            var editCommand = new EditLanguageCommand();
            editCommand.Id = publisher.Id;
            editCommand.Name = publisher.Name;
            editCommand.ShortName = publisher.ShortName;

            return View(editCommand);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.languages.edit")]
        public async Task<IActionResult> Edit(int id, EditLanguageCommand command)
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
        [Authorize(Policy = "admin.languages.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, RemoveLanguageCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(int id)
        {
            return db.Languages.Any(e => e.Id == id);
        }
    }
}
