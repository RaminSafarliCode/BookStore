using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities.Chat
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }
    }
}
