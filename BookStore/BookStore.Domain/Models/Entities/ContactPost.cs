using BookStore.Application.AppCode.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class ContactPost : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }

        public string Answer { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int? AnswerByUserId { get; set; }

        [NotMapped]
        public string EmailSubject { get; set; }

    }
}
