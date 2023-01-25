﻿using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class BlogPostTagItem : BaseEntity
    {
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public int BlogPostId { get; set; }
        public virtual BlogPost BlogPost { get; set; }
    }
}
