using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using BookStore.Domain.Business.AboutModule;
using BookStore.Domain.Business.BlogPostModule;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutCompanyController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public AboutCompanyController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [Authorize("admin.aboutcompany.index")]
        public async Task<IActionResult> Index(AboutGetAllQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize("admin.aboutcompany.create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);

                if (response.Error == false)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(command);
        }

        [Authorize("admin.aboutcompany.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await db.AboutCompany.FirstOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        [HttpPost]
        [Authorize("admin.aboutcompany.edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response.Error == false)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }



        private bool AboutExists(int id)
        {
            return db.AboutCompany.Any(e => e.Id == id);
        }
    }
}