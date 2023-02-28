using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities.Chat;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.ChatModule
{
    public class MessagesQuery : IRequest<List<Message>>
    {
        public class MessagesQueryHandler : IRequestHandler<MessagesQuery, List<Message>>
        {
            private readonly BookStoreDbContext db;

            public MessagesQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<List<Message>> Handle(MessagesQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Messages
                .Where(bp => bp.DeletedDate == null)
                .ToListAsync(cancellationToken);

                return data;
            }
        }


    }
}
