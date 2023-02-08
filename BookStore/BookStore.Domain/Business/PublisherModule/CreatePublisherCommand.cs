using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
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

            public CreatePublisherCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Publisher> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
            {
                var publisher = new Publisher();

                publisher.Name = request.Name;

                await db.Publishers.AddAsync(publisher, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return publisher;
            }
        }
    }


}
