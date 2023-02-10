using BookStore.Domain.Business.BookModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        private readonly IMediator mediator;

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BooksPagedQuery query)
        {
            var book = await mediator.Send(query);
            return View(book);
        }
    }
}
