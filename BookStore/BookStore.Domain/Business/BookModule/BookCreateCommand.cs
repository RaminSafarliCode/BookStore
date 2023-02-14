using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BookCreateCommand : IRequest<Book>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }

        public string StockKeepingUnit { get; set; }
        public string Summary { get; set; }
        public int Page { get; set; }

        public int LanguageId { get; set; }

        public int PublisherId { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public class BookCreateCommandHandler : IRequestHandler<BookCreateCommand, Book>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;
            private readonly IConfiguration configuration;
            private readonly IActionContextAccessor ctx;

            public BookCreateCommandHandler(BookStoreDbContext db, IHostEnvironment env, IConfiguration configuration, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.configuration = configuration;
                this.ctx = ctx;
            }
            public async Task<Book> Handle(BookCreateCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                    return null;

                var book = new Book();
                book.CreatedByUserId = ctx.GetCurrentUserId();
                book.Name = request.Name;
                book.StockKeepingUnit = request.StockKeepingUnit;
                book.Price = request.Price;
                book.Summary = request.Summary;
                book.StockKeepingUnit = request.StockKeepingUnit;
                book.Page = request.Page;
                book.LanguageId = request.LanguageId;
                book.PublisherId = request.PublisherId;
                book.AuthorId = request.AuthorId;
                book.CategoryId = request.CategoryId;

                string extension = Path.GetExtension(request.Image.FileName);//.jpg

                request.ImagePath = $"book-{Guid.NewGuid().ToString().ToLower()}{extension}";

                var folder = configuration["uploads:folder"];

                string fullPath = null;

                if (!string.IsNullOrWhiteSpace(folder))
                {
                    fullPath = folder.GetImagePhysicalPath(request.ImagePath);
                }
                else
                {
                    fullPath = env.GetImagePhysicalPath(request.ImagePath);
                }

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                book.ImagePath = request.ImagePath;

                await db.Books.AddAsync(book, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);


                return book;
            }
        }
    }
}
