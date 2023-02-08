using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.PublisherModule
{
    public class RemovePublisherCommand : IRequest<Publisher>
    {
        public int Id { get; set; }

        public class RemovePublisherCommandHandler : IRequestHandler<RemovePublisherCommand, Publisher>
        {
            private readonly BookStoreDbContext db;

            public RemovePublisherCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Publisher> Handle(RemovePublisherCommand request, CancellationToken cancellationToken)
            {
                var data = db.Publishers
                    .FirstOrDefault(m => m.Id == request.Id && m.DeletedDate == null);

                if (data == null)
                {
                    throw new Exception("Publisher not found!");
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}
