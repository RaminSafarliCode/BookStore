using BookStore.Domain.Business.BookModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class NewReleaseBooksViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public NewReleaseBooksViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new NewReleaseBookQuery() { Size = 3 };
            var books = await mediator.Send(query);

            return View(books);
        }
    }
}
