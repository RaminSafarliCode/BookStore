using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Services;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookStore.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactPostsController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly EmailService emailService;

        public ContactPostsController(BookStoreDbContext db, EmailService emailService)
        {
            this.db = db;
            this.emailService = emailService;
        }

        [Authorize("admin.contactposts.index")]
        public async Task<IActionResult> Index()
        {
            var requests = await db.ContactPosts.ToListAsync();
            return View(requests);
        }

        [Authorize("admin.contactposts.details")]
        public async Task<IActionResult> Details(int id)
        {
            var request = await db.ContactPosts.FirstOrDefaultAsync(request => request.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }


        [HttpPost]
        [Authorize("admin.contactposts.reply")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, [FromForm][Bind("Answer, Email")] ContactPost model,[FromForm] Test emailSubject)
        {
            var entity = await db.ContactPosts.FirstOrDefaultAsync(bp => bp.Id == id && bp.AnswerDate == null);

            entity.AnswerDate = DateTime.UtcNow.AddHours(4);
            entity.Answer = model.Answer;
            entity.AnswerByUserId = User.GetCurrentUserId();
            await db.SaveChangesAsync();

            await emailService.SendMailAsync(model.Email, emailSubject.ToString(), model.Answer);

            return Json(new
            {
                error = false,
                message = "Your answer has been sended"
            });
        }

        #region First version
        //[HttpPost]
        //public async Task<IActionResult> Reply(int id, [FromForm][Bind("FirstName, LastName, Email, Answer")] ContactPost model, [FromBody] Test subject)
        //{
        //    var entity = await db.ContactPosts.FirstOrDefaultAsync(bp => bp.Id == id && bp.AnswerDate == null);

        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new
        //        {
        //            error = true,
        //            message = "Xeta"
        //        });
        //    }

        //    entity.AnswerDate = DateTime.UtcNow.AddHours(4);
        //    entity.Answer = model.Answer;
        //    //entity.EmailSubject = model.EmailSubject;
        //    await db.SaveChangesAsync();

        //    await emailService.SendMailAsync(model.Email, subject.ToString(), model.Answer);

        //    //return RedirectToAction(nameof(Index));
        //    return Json(new
        //    {
        //        error = false,
        //        message = "Your answer has been sended"
        //    });

        //}
        #endregion

    }
}
