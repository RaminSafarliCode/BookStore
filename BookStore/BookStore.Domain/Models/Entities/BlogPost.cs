using BookStore.Application.AppCode.Infrastructure;
using System;
using System.Collections.Generic;

namespace BookStore.Domain.Models.Entities
{
    public class BlogPost : BaseEntity, IPageable
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImagePath { get; set; }
        public string Slug { get; set; }
        public DateTime? PublishedDate { get; set; }

        public virtual ICollection<BlogPostComment> Comments { get; set; }

        public int BlogPostCategoryId { get; set; }
        public virtual BlogPostCategory BlogPostCategory { get; set; }

        public virtual ICollection<BlogPostTagItem> TagCloud { get; set; }
    }
}
