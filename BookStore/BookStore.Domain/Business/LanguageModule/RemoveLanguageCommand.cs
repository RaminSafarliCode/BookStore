using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.LanguageModule
{
    public class RemoveLanguageCommand : IRequest<Language>
    {
        public int Id { get; set; }

        public class RemoveLanguageCommandHandler : IRequestHandler<RemoveLanguageCommand, Language>
        {
            private readonly BookStoreDbContext db;

            public RemoveLanguageCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Language> Handle(RemoveLanguageCommand request, CancellationToken cancellationToken)
            {
                var data = db.Languages
                    .FirstOrDefault(m => m.Id == request.Id && m.DeletedDate == null);

                if (data == null)
                {
                    throw new Exception("Language not found!");
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}
