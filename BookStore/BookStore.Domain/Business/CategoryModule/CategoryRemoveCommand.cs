﻿using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.Domain.Business.CategoryModule
{

    public class CategoryRemoveCommand : IRequest<Category>
    {
        public int Id { get; set; }

        public class CategoryRemoveCommandHandler : IRequestHandler<CategoryRemoveCommand, Category>
        {
            private readonly BookStoreDbContext db;

            public CategoryRemoveCommandHandler(BookStoreDbContext db)
            {
                this.db = db;
            }

            public async Task<Category> Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
            {
                var data = db.Categories
                    .Include(c => c.Children.Where(ch => ch.DeletedDate == null))
                    .FirstOrDefault(m => m.Id == request.Id && m.DeletedDate == null);

                if (data == null)
                {
                    return null;
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);

                var children = data.Children.Where(c => c.ParentId == data.Id);

                foreach (var item in children)
                {
                    item.DeletedDate = DateTime.UtcNow.AddHours(4);
                }

                await db.SaveChangesAsync(cancellationToken);
                return data;
            }
        }
    }
}
