﻿using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Business.ChatModule;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BookStore.Domain.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;
        static ConcurrentDictionary<int, string> clients = new ConcurrentDictionary<int, string>();

        public ChatHub(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var httpContext = Context.GetHttpContext();
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var userId = httpContext.User.GetCurrentUserId();

                clients.AddOrUpdate(userId, Context.ConnectionId, (k, v) => v);

                var command = new HubUserConnectCommand
                {
                    HubContext = Context,
                    HubGroups = Groups
                };

                await mediator.Send(command);

            }


        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var cId = Context.ConnectionId;


            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var command = new SendToGroupCommand
            {
                HubContext = Context,
                GroupName = "CallCenter",
                Message = message
            };

            var response = await mediator.Send(command);

            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.GetCurrentUserId();

            await Clients.GroupExcept(command.GroupName, Context.ConnectionId)
                .SendAsync("ReceiveFromOperator", JsonConvert.SerializeObject(response));
        }

        public async Task SendFromOperator(string message, int toUserId)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.User.GetCurrentUserId();

            var command = new SendToUserCommand
            {
                HubContext = Context,
                UserId = toUserId,
                Message = message
            };

            var response = await mediator.Send(command);

            if (response != null && clients.TryGetValue(response.ToId.Value, out string toUserConnectionId))
            {
                await Clients.User(toUserConnectionId)
                    .SendAsync("ReceiveFromClient", response.ToId.Value, response.Text);
            }
        }
    }
}
