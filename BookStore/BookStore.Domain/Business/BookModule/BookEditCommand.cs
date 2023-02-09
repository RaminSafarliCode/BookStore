using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.BookModule
{
    public class BookEditCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
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

        public class BookEditCommandHandler : IRequestHandler<BookEditCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;
            private readonly IActionContextAccessor ctx;

            public BookEditCommandHandler(BookStoreDbContext db, IHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(BookEditCommand request, CancellationToken cancellationToken)
            {

                var book = await db.Books
                    .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (book == null)
                {
                    throw new Exception("Book not found");
                }

                book.Name = request.Name;
                book.Price = request.Price;
                book.StockKeepingUnit = request.StockKeepingUnit;
                book.Summary = request.Summary;
                book.Page = request.Page;
                book.LanguageId = request.LanguageId;
                book.PublisherId = request.PublisherId;
                book.AuthorId = request.AuthorId;
                book.CategoryId = request.CategoryId;

                if (request.Image == null)
                    goto end;

                string extension = Path.GetExtension(request.Image.FileName);//.jpg

                request.ImagePath = $"book-{Guid.NewGuid().ToString().ToLower()}{extension}";
                string fullPath = env.GetImagePhysicalPath(request.ImagePath);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                env.ArchiveImage(book.ImagePath);

                book.ImagePath = request.ImagePath;

            end:

                await db.SaveChangesAsync(cancellationToken);

                //return book;

                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }
        }
    }
}
