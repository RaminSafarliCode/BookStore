using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
