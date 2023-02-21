using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Business.BookModule;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using Cozy.Domain.Models.ViewModels.OrderViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IMediator mediator;

        public BookController(BookStoreDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BookFilterQuery query)
        {
            var response = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                //return Json(response);
                return PartialView("_Books", response);
            }

            return View(response);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(BookSingleQuery query)
        {
            var book = await mediator.Send(query);
            if (book == null)
            {
                return NotFound();
            }

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

        [HttpPost]
        public async Task<IActionResult> SetBookRate(SetRateCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }

        //[Route("/checkout")]
        public async Task<IActionResult> Checkout(BookBasketQuery query)
        {
            var response = await mediator.Send(query);

            return View(new OrderViewModel
            {
                BasketBooks = response
            });
        }

        [HttpPost]
        //[Route("/checkout")]
        public async Task<IActionResult> Checkout(OrderViewModel vm, int[] productIds, int[] quantities)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(vm.OrderDetails);

                await db.SaveChangesAsync();

                vm.OrderDetails.OrderBooks = new List<OrderBook>();

                for (int i = 0; i < productIds.Length; i++)
                {
                    var product = db.Books.Find(productIds[i]);
                    vm.OrderDetails.OrderBooks.Add(new OrderBook
                    {
                        OrderId = vm.OrderDetails.Id,
                        BookId = product.Id,
                        Quantity = quantities[i]
                    });
                }
                await db.SaveChangesAsync();

                var response = new
                {
                    error = false,
                    message = "Your order was completed"
                };

                return Json(response);
            }

            var responseError = new
            {
                error = true,
                message = "The error was occurred while completing your order",
                state = ModelState.GetErrors()
            };
            return Json(responseError);
        }
    }
}
