using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
    public class CreateAuthorCommand : IRequest<Author>
    {
        public string Name { get; set; }
        public string Biography { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Image { get; set; }

        public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Author>
        {
            private readonly BookStoreDbContext db;
            private readonly IConfiguration configuration;
            private readonly IHostEnvironment env;

            public CreateAuthorCommandHandler(BookStoreDbContext db, IConfiguration configuration, IHostEnvironment env)
            {
                this.db = db;
                this.configuration = configuration;
                this.env = env;
            }

            public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
            {
                var author = new Author();

                author.Name = request.Name;
                author.Biography = request.Biography;

                string extension = Path.GetExtension(request.Image.FileName);//.jpg

                request.ImagePath = $"blogpost-{Guid.NewGuid().ToString().ToLower()}{extension}";

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

                author.ImagePath = request.ImagePath;

                await db.Authors.AddAsync(author, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return author;
            }
        }
    }

    
}
