using BookStore.Domain.Models.DataContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubscribesController : Controller
    {
        private readonly BookStoreDbContext db;

        public SubscribesController(BookStoreDbContext db)
        {
            this.db = db;
        }

        [Authorize(Policy = "admin.subscribes.index")]
        public async Task<IActionResult> Index()
        {
            return View(await db.Subscribes.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.subscribes.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscribe = await db.Subscribes.FindAsync(id);
            db.Subscribes.Remove(subscribe);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
