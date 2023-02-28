using BookStore.Domain.Models.Entities;
using System.Collections.Generic;

namespace BookStore.Domain.Models.ViewModels.OrderViewModel
{
    public class OrderViewModel
    {
        public IEnumerable<Basket> BasketBooks { get; set; }

        public Order OrderDetails { get; set; }
    }
}
