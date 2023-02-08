using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.AuthorModule
{
    public class EditAuthorCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }

        public class EditAuthorCommandHandler : IRequestHandler<EditAuthorCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;

            public EditAuthorCommandHandler(BookStoreDbContext db, IHostEnvironment env)
            {
                this.db = db;
                this.env = env;
            }

            public async Task<JsonResponse> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
            {
                var author = await db.Authors
                    .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (author == null)
                {
                    throw new Exception("Author not found");
                }

                author.Name = request.Name;
                author.Biography = request.Biography;

                if (request.Image == null)
                    goto end;

                string extension = Path.GetExtension(request.Image.FileName);//.jpg

                request.ImagePath = $"blogpost-{Guid.NewGuid().ToString().ToLower()}{extension}";
                string fullPath = env.GetImagePhysicalPath(request.ImagePath);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                env.ArchiveImage(author.ImagePath);

                author.ImagePath = request.ImagePath;

            end:

                await db.SaveChangesAsync(cancellationToken);

                //return author;

                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }
        }
    }

}
