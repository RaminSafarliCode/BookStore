using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.CategoryModule
{
    public class CategoryCreateCommand : IRequest<Category>
    {
        [Required]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public class CategoryCreateCommandHandler : IRequestHandler<CategoryCreateCommand, Category>
        {
            private readonly BookStoreDbContext db;

            public CategoryCreateCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Category> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = new Category();


                entity.Name = request.Name;
                entity.ParentId = request.ParentId;
                

                await db.Categories.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }


        }
    }

}
