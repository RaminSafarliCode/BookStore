﻿using BookStore.Domain.Business.AuthorModule;
using BookStore.Domain.Business.BookModule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class PopularAuthorsViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public PopularAuthorsViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var query = new PopularAuthorsQuery() { Size = 7 };
            var books = await mediator.Send(query);

            return View(books);
        }
    }
}
