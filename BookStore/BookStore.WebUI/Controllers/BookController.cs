using BookStore.Application.AppCode.Extenstions;
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


        public async Task<IActionResult> Wishlist(WishlistQuery query)
        {
            var favs = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_WishlistBody", favs);
            }

            return View(favs);
        }



        //[Route("/basket")]
        public async Task<IActionResult> Basket(BookBasketQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);
        }

        [HttpPost]
        //[Route("/basket")]
        public async Task<IActionResult> Basket(AddToBasketCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(RemoveFromBasketCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeBasketQuantity(ChangeBasketQuantityCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }
    }

    
}
