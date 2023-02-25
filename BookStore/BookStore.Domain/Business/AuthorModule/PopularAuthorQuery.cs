//using BookStore.Domain.Models.DataContexts;
//using BookStore.Domain.Models.Entities;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace BookStore.Domain.Business.AuthorModule
//{
//    public class PopularAuthorQuery : IRequest<List<Author>>
//    {
//        public int Size { get; set; }


//        public class PopularAuthorQueryHandler : IRequestHandler<PopularAuthorQuery, List<Author>>
//        {
//            private readonly BookStoreDbContext db;

//            public PopularAuthorQueryHandler(BookStoreDbContext db)
//            {
//                this.db = db;
//            }

//            public async Task<List<Author>> Handle(PopularAuthorQuery request, CancellationToken cancellationToken)
//            {
//                var mostOrderedAuthorIds = await db.Books
//                    .GroupBy(b => b.AuthorId)
//                    .Select(g => new
//                    {
//                        AuthorId = g.Key,
//                        OrderCount = g.SelectMany(b => b.OrderBooks).Count()
//                    })
//                    .OrderByDescending(x => x.OrderCount)
//                    .Select(x => x.AuthorId)
//                    .Take(request.Size < 2 ? 2 : request.Size)
//                    .ToListAsync();

//                var mostOrderedAuthors = await db.Authors
//                    .Include(a => a.Books)
//                    .Where(a => mostOrderedAuthorIds.Contains(a.Id))
//                    .ToListAsync();

//                return mostOrderedAuthors;
//            }
//        }
//    }
//}
