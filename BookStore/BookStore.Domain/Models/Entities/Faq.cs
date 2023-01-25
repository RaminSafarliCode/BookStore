using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class Faq : BaseEntity
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
