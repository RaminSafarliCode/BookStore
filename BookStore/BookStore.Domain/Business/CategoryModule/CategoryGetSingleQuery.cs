using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.CategoryModule
{
    public class CategoryGetSingleQuery : IRequest<Category>
    {
        public int Id { get; set; }

        public class CategoryGetSingleQueryHandler : IRequestHandler<CategoryGetSingleQuery, Category>
        {
            private readonly BookStoreDbContext db;

            public CategoryGetSingleQueryHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Category> Handle(CategoryGetSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Categories
                    .Include(c => c.Parent)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                return data;
            }
        }

    }
}
