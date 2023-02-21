using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class SearchPanelViewComponent : ViewComponent
    {

        private readonly BookStoreDbContext db;

        public SearchPanelViewComponent(BookStoreDbContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = new SearchPanelViewModel();

            vm.Authors = db.Authors.Where(d => d.DeletedDate == null).Distinct().ToArray();
            vm.Languages = db.Languages.Where(d => d.DeletedDate == null).Distinct().ToArray();
            vm.Publishers = db.Publishers.Where(d => d.DeletedDate == null).Distinct().ToArray();
            vm.Categories = db.Categories.Where(d=>d.DeletedDate == null).Distinct().ToArray();

            var priceRange = db.Books
                .Select(pc => pc.Price)
                .ToArray();

            vm.Min = (int)Math.Floor(priceRange.Min());
            vm.Max = (int)Math.Ceiling(priceRange.Max());

            return View(vm);
        }
    }
}
