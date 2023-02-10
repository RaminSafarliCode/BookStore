using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using BookStore.Domain.Business.BookModule;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public BooksController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        // GET: Admin/Books

        [Authorize(Policy = "admin.books.index")]
        public async Task<IActionResult> Index(BooksPagedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        // GET: Admin/Books/Details/5

        [Authorize(Policy = "admin.books.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Include(b => b.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [Authorize(Policy = "admin.books.create")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(db.Authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(db.Publishers, "Id", "Name");
            ViewData["LanguageId"] = new SelectList(db.Languages, "Id", "ShortName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Policy = "admin.books.create")]
        public async Task<IActionResult> Create(BookCreateCommand command)
        {
            var response = await mediator.Send(command);

            if (!ModelState.IsValid)
            {
                ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", command.AuthorId);
                ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", command.PublisherId);
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "ShortName");
                return View(command);
            }

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Policy = "admin.books.edit")]
        public async Task<IActionResult> Edit(BookSingleQuery query)
        {
            var book = await mediator.Send(query);
            if (book == null)
            {
                return NotFound();
            }

            var command = new BookEditCommand();
            command.Name = book.Name;
            command.Price = book.Price;
            command.StockKeepingUnit = book.StockKeepingUnit;
            command.Summary = book.Summary;
            command.Page = book.Page;
            command.LanguageId = book.LanguageId;
            command.PublisherId = book.PublisherId;
            command.AuthorId = book.AuthorId;
            command.CategoryId = book.CategoryId;
            command.ImagePath = book.ImagePath;



            ViewData["AuthorId"] = new SelectList(db.Authors, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(db.Publishers, "Id", "Name");
            ViewData["LanguageId"] = new SelectList(db.Languages, "Id", "ShortName");
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.books.edit")]
        public async Task<IActionResult> Edit(int id, BookEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (!ModelState.IsValid)
            {
                ViewData["AuthorId"] = new SelectList(db.Authors, "Id", "Name");
                ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
                ViewData["PublisherId"] = new SelectList(db.Publishers, "Id", "Name");
                ViewData["LanguageId"] = new SelectList(db.Languages, "Id", "ShortName");
                return View(command);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.books.delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, BookRemoveCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return db.Books.Any(e => e.Id == id);
        }
    }
}
