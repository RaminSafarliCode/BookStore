using BookStore.Domain.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.AppCode.ViewComponents
{
    public class OrderInfoViewComponent : ViewComponent
    {
        private readonly BookStoreDbContext db;

        public OrderInfoViewComponent(BookStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            var data = await db.Basket
                .Include(d => d.Book)
                .ToListAsync();

            if (data == null)
            {
                return null;
            }

            return View(await Task.FromResult(data));
        }
    }
}
