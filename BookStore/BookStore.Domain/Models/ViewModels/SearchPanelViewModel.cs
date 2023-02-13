using BookStore.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Models.ViewModels
{
    public class SearchPanelViewModel
    {
        public Author[] Authors { get; set; }
        public Publisher[] Publishers { get; set; }
        public Language[] Languages { get; set; }
        public Category[] Categories { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }
    }
}
