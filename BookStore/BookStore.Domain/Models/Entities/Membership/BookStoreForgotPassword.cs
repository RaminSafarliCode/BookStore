using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities.Membership
{
    public class BookStoreForgotPassword : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
