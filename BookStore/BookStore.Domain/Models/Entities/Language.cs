using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
