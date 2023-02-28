using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.ViewModels
{
    public class MessageViewModel
    {
        public int FriendId { get; set; }
        public string FriendName { get; set; }
        public string LastMessage { get; set; }
        public string Date { get; set; }
    }
}
