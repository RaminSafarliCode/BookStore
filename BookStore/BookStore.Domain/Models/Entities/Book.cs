using BookStore.Application.AppCode.Infrastructure;
using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class Book : BaseEntity, IPageable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string StockKeepingUnit { get; set; }
        public double Rate { get; set; }
        public string Summary { get; set; }
        public int Page { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<OrderBook> OrderBooks { get; set; }
    }
}
