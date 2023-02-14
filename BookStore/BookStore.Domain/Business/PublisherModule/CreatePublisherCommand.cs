using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.PublisherModule
{
    public class CreatePublisherCommand : IRequest<Publisher>
    {
        public string Name { get; set; }

        public class CreatePublisherCommandHandler : IRequestHandler<CreatePublisherCommand, Publisher>
        {
            private readonly BookStoreDbContext db;
            private readonly IActionContextAccessor ctx;

            public CreatePublisherCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<Publisher> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                    return null;

                var publisher = new Publisher();

                publisher.Name = request.Name;
                publisher.CreatedByUserId = ctx.GetCurrentUserId();

                await db.Publishers.AddAsync(publisher, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return publisher;
            }
        }
    }


}
