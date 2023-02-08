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

namespace BookStore.Domain.Business.PublisherModule
{
    public class EditPublisherCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class EditPublisherCommandHandler : IRequestHandler<EditPublisherCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;

            public EditPublisherCommandHandler(BookStoreDbContext db, IHostEnvironment env)
            {
                this.db = db;
                this.env = env;
            }

            public async Task<JsonResponse> Handle(EditPublisherCommand request, CancellationToken cancellationToken)
            {
                var publisher = await db.Publishers
                    .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (publisher == null)
                {
                    throw new Exception("Publisher not found");
                }

                publisher.Name = request.Name;

                await db.SaveChangesAsync(cancellationToken);

                //return publisher;

                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }
        }
    }

}
