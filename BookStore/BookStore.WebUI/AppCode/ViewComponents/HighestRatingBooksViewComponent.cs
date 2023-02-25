using BookStore.Domain.Business.BookModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class HighestRatingBooksViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public HighestRatingBooksViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new BestsellerBooksQuery() { Size = 6 };
            var books = await mediator.Send(query);

            return View(books);
        }
    }
}
