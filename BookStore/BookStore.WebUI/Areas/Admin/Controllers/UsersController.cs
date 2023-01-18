using BookStore.Domain.Models.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly BookStoreDbContext db;

        public UsersController(BookStoreDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var data = await db.Users.ToListAsync();
            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await db.Users.FirstOrDefaultAsync(user => user.Id == id);
            return View(data);
        }   
    }
}
