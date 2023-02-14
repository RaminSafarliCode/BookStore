using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            private readonly IActionContextAccessor ctx;

            public CreateLanguageCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<Language> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                    return null;

                var publisher = new Language();

                publisher.CreatedByUserId = ctx.GetCurrentUserId();
                publisher.Name = request.Name;
                publisher.ShortName = request.ShortName;

                await db.Languages.AddAsync(publisher, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return publisher;
            }
        }
    }


}
