using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.LanguageModule
{
    public class CreateLanguageCommand : IRequest<Language>
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, Language>
        {
            private readonly BookStoreDbContext db;

            public CreateLanguageCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Language> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
            {
                var publisher = new Language();

                publisher.Name = request.Name;
                publisher.ShortName = request.ShortName;

                await db.Languages.AddAsync(publisher, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return publisher;
            }
        }
    }


}
