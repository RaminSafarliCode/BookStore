using BookStore.Domain.Business.BlogPostModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class TrendingBlogPostsViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public TrendingBlogPostsViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new TrendingPostQuery() { Size = 4};
            var posts = await mediator.Send(query);

            return View(posts);
        }
    }
}
