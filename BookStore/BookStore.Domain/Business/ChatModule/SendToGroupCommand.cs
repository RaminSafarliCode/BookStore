using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities.Chat;
using BookStore.Domain.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.ChatModule
{
    public class SendToGroupCommand : IRequest<MessageViewModel>
    {
        public HubCallerContext HubContext { get; set; }
        public string GroupName { get; set; }
        public string Message { get; set; }
        public class SendToGroupCommandHandler : IRequestHandler<SendToGroupCommand, MessageViewModel>
        {
            private readonly BookStoreDbContext db;

            public SendToGroupCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<MessageViewModel> Handle(SendToGroupCommand request, CancellationToken cancellationToken)
            {
                var httpContext = request.HubContext.GetHttpContext();

                var group = await db.ChatGroups
                    .FirstOrDefaultAsync(cg=>cg.Name.Equals(request.GroupName), cancellationToken);

                var message = new Message();

                message.CreatedByUserId = httpContext.User.GetCurrentUserId();
                message.GroupId = group.Id;
                message.Text = request.Message;

                await db.Messages.AddAsync(message, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                message = await db.Messages
                     .Include(m => m.CreatedByUser)
                    .FirstOrDefaultAsync(m => m.Id == message.Id, cancellationToken);

                return new MessageViewModel
                {
                    FriendId = message.CreatedByUserId.Value,
                    FriendName = message.CreatedByUser.UserName,
                    LastMessage = message.Text,
                    Date = message.CreatedDate.ToString("HH:mm")
                };
            }
        }
    }
}
