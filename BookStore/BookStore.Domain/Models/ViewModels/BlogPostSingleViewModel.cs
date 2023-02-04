using BookStore.Domain.Models.Entities;
using System.Collections.Generic;

namespace BookStore.Domain.Models.ViewModels
{
    public class BlogPostSingleViewModel
    {
        public BlogPost Post { get; set; }
        public List<BlogPostCategory> Categories { get; set; }
        public List<BlogPostComment> Comments { get; set; }
    }
}
