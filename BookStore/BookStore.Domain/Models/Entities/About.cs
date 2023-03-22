using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.Entities
{
    public class About 
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WorkHours { get; set; }
        public string Mail { get; set; }
        public string FacebookAddress  { get; set; }
        public string TwitterAddress { get; set; }
        public string LinkedinAddress { get; set; }
    }
}
