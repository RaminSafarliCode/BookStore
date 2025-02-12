﻿using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            private readonly IActionContextAccessor ctx;

            public CategoryCreateCommandHandler(BookStoreDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<Category> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
            {
                if (!ctx.IsValid())
                    return null;

                var entity = new Category();

                entity.CreatedByUserId = ctx.GetCurrentUserId();
                entity.Name = request.Name;
                entity.ParentId = request.ParentId;
                

                await db.Categories.AddAsync(entity, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }

}
