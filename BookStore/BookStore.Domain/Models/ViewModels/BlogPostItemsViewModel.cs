using BookStore.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.ViewModels
{
    public class BlogPostItemsViewModel
    {
        public BlogPost BlogPost { get; set; }

        public ICollection<BlogPostReact> BlogPostReacts { get; set; }
    }
}
