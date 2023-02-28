using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities.Chat;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.ChatModule
{
    public class SendToUserCommand : IRequest<Message>
    {
        public HubCallerContext HubContext { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }

        public class SendToUserCommandHandler : IRequestHandler<SendToUserCommand, Message>
        {
            private readonly BookStoreDbContext db;

            public SendToUserCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }
            public async Task<Message> Handle(SendToUserCommand request, CancellationToken cancellationToken)
            {
                var httpContext = request.HubContext.GetHttpContext();

                if (httpContext.User.Identity.IsAuthenticated)
                {

                    var message = new Message();

                    message.CreatedByUserId = httpContext.User.GetCurrentUserId();
                    message.ToId = request.UserId;
                    message.Text = request.Message;

                    await db.Messages.AddAsync(message, cancellationToken);
                    await db.SaveChangesAsync(cancellationToken);


                    return message;
                }

                return null;

            }
        }
    }
}
