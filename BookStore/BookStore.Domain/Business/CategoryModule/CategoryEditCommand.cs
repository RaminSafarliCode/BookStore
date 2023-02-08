using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.CategoryModule
{

    public class CategoryEditCommand : IRequest<Category>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public class CategoryEditCommandHandler : IRequestHandler<CategoryEditCommand, Category>
        {
            private readonly BookStoreDbContext db;

            public CategoryEditCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Category> Handle(CategoryEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Categories
                       .Include(c => c.Parent)
                       .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);
                if (entity == null)
                {
                    return null;
                }

                entity.Name = request.Name;
                entity.ParentId = request.ParentId;


                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }


        }
    }
}
