using BookStore.Domain.Business.BookModule;
using BookStore.Domain.Business.ChatModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class ChatListViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public ChatListViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new MessagesQuery();
            var messages = await mediator.Send(query);

            return View(messages);
        }
    }
}
