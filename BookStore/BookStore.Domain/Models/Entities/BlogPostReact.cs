using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class BlogPostReact : BaseEntity
    {
        public int BlogPostId { get; set; }
        public virtual BlogPost BlogPost { get; set; }

    }
}
