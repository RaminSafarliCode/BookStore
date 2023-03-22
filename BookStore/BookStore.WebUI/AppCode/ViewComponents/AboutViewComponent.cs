using BookStore.Domain.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.ViewComponents.About
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly BookStoreDbContext db;

        public AboutViewComponent(BookStoreDbContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            var data = await db.AboutCompany.FirstOrDefaultAsync();

            if (data == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(viewName))
            {
                return View(viewName, await Task.FromResult(data));
            }

            return View(await Task.FromResult(data));
        }
    }
}
