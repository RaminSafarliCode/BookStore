using BookStore.Domain.Business.BookModule;
using BookStore.Domain.Models.DataContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.ViewComponents.About
{
    public class BasketCounterViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public BasketCounterViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new BookBasketQuery();
            var response = await mediator.Send(query);
            return View(response);
        }
    }
}
