﻿using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class Tag : BaseEntity
    {
        public string Text { get; set; }
        public virtual ICollection<BlogPostTagItem> TagCloud { get; set; }
    }
}
