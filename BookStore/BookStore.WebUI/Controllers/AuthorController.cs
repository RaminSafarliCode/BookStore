using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Business.AuthorModule;
using BookStore.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    [AllowAnonymous]
    public class AuthorController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public AuthorController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(GetAllAuthorQuery query)
        {
            var response = await mediator.Send(query);

            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("_Body", response);
            //}

            return View(response);
        }

        [AllowAnonymous]
        [Route("/author/{id}")]
        public async Task<IActionResult> Details(GetSingleAuthorQuery query)
        {
            //var author = await db.Authors.FirstOrDefaultAsync(entity => entity.Id == id);

            //if (author == null)
            //{
            //    return NotFound();
            //}

            //return View(author);


            var author = await mediator.Send(query);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }
    }
}
