using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class OrderBook : BaseEntity
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int Quantity { get; set; }
    }
}
