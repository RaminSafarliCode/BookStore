using MediatR;
using Microsoft.EntityFrameworkCore;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using BookStore.Application.AppCode.Infrastructure;
using System.Linq;

namespace BookStore.Domain.Business.AboutModule
{
    public class AboutEditCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WorkHours { get; set; }
        public string Mail { get; set; }
        public string FacebookAddress { get; set; }
        public string TwitterAddress { get; set; }
        public string LinkedinAddress { get; set; }



        public class AboutEditCommandHandler : IRequestHandler<AboutEditCommand, JsonResponse>
        {
            private readonly BookStoreDbContext db;

            public AboutEditCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<JsonResponse> Handle(AboutEditCommand request, CancellationToken cancellationToken)
            {
                var data = await db.AboutCompany
                    .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

                if (data == null)
                {
                    return null;
                }

                data.Address = request.Address;
                data.Phone = request.Phone;
                data.WorkHours = request.WorkHours;
                data.Mail = request.Mail;
                data.FacebookAddress = request.FacebookAddress;
                data.TwitterAddress = request.TwitterAddress;
                data.LinkedinAddress = request.LinkedinAddress;

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