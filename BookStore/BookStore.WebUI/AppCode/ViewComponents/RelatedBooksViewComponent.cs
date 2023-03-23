using BookStore.Domain.Business.BookModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class RelatedBooksViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public RelatedBooksViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName)
        {
            var query = new RelatedBooksQuery() {Category = categoryName};
            var books = await mediator.Send(query);

            return View(books);
        }
    }
}
