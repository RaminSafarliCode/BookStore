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

            vm.Authors = db.Authors.Distinct().ToArray();
            vm.Languages = db.Languages.Distinct().ToArray();
            vm.Publishers = db.Publishers.Distinct().ToArray();
            vm.Categories = db.Categories.Distinct().ToArray();

            //vm.Colors = db.ProductCatalog
            //    .Include(pc => pc.ProductColor)
            //    .Select(pc => pc.ProductColor)
            //    .Distinct()
            //    .ToArray();

            //vm.Sizes = db.ProductCatalog
            //    .Include(pc => pc.ProductSize)
            //    .Select(pc => pc.ProductSize)
            //    .Distinct()
            //    .ToArray();

            //vm.Materials = db.ProductCatalog
            //    .Include(pc => pc.ProductMaterial)
            //    .Select(pc => pc.ProductMaterial)
            //    .Distinct()
            //    .ToArray();

            //vm.ProductTypes = db.ProductCatalog
            //    .Include(pc => pc.ProductType)
            //    .Select(pc => pc.ProductType)
            //    .Distinct()
            //    .ToArray();

            //vm.Brands = db.ProductCatalog
            //    .Include(pc => pc.Product)
            //    .ThenInclude(pc => pc.Brand)
            //    .Select(pc => pc.Product.Brand)
            //    .Distinct()
            //    .ToArray();

            var priceRange = db.Books
                .Select(pc => pc.Price)
                .ToArray();

            vm.Min = (int)Math.Floor(priceRange.Min());
            vm.Max = (int)Math.Ceiling(priceRange.Max());

            return View(vm);
        }
    }
}
