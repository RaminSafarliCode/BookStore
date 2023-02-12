using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class BookRate : AutitableEntity
    {
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public byte Rate { get; set; }
    }
}
