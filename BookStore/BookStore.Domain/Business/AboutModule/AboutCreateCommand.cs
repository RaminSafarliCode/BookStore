using BookStore.Domain.AppCode.Extensions;
using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace BookStore.Domain.Business.BlogPostModule
{
    public class AboutCreateCommand : IRequest<JsonResponse>
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WorkHours { get; set; }
        public string Mail { get; set; }
        public string FacebookAddress { get; set; }
        public string TwitterAddress { get; set; }
        public string LinkedinAddress { get; set; }

        public class AboutCreateCommandHandler : IRequestHandler<AboutCreateCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;
            private readonly IHostEnvironment env;

            public AboutCreateCommandHandler(BookStoreDbContext db, IHostEnvironment env)
            {
                this.db = db;
            }

            
            public async Task<JsonResponse> Handle(AboutCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = new About();

                entity.Address = request.Address;
                entity.Phone = request.Phone;
                entity.WorkHours = request.WorkHours;
                entity.Mail = request.Mail;
                entity.FacebookAddress = request.FacebookAddress;
                entity.TwitterAddress = request.TwitterAddress;
                entity.LinkedinAddress = request.LinkedinAddress;

                await db.AboutCompany.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }
        }
    }
}
