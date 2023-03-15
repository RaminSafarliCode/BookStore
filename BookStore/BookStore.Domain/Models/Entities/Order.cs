using BookStore.Domain.Models.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class Order : BaseEntity
    {
        [Required(ErrorMessage = "This field is required to be filled in!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "This field is required to be filled in!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "This field is required to be filled in!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This field is required to be filled in!")]
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public decimal TotalPrice { get; set; }

        public bool IsDelivered { get; set; } = false;
        public ICollection<OrderBook> OrderBooks { get; set; }
    }
}
