using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.AuthorModule
{
    public class RemoveAuthorCommand : IRequest<Author>
    {
        public int Id { get; set; }

        public class RemoveAuthorCommandHandler : IRequestHandler<RemoveAuthorCommand, Author>
        {
            private readonly BookStoreDbContext db;

            public RemoveAuthorCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Author> Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
            {
                var data = db.Authors
                    .FirstOrDefault(m => m.Id == request.Id && m.DeletedDate == null);

                if (data == null)
                {
                    throw new Exception("Author not found!");
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}
