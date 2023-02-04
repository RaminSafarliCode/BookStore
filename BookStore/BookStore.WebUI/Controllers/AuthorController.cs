using BookStore.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
