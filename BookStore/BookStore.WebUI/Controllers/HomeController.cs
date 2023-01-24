using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Services;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly CryptoService crypto;
        private readonly EmailService emailService;
        private readonly IValidator<ContactPost> contactPostValidator;

        public HomeController(BookStoreDbContext db, CryptoService crypto, EmailService emailService, IValidator<ContactPost> contactPostValidator)
        {
            this.db = db;
            this.crypto = crypto;
            this.emailService = emailService;
            this.contactPostValidator = contactPostValidator;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactPost model)
        {
            var validateResult = contactPostValidator.Validate(model);

            if (validateResult.IsValid)
            {
                db.ContactPosts.Add(model);
                db.SaveChanges();

                ModelState.Clear();
                return Json(new
                {
                    error = false,
                    message = "Your request has been recorded!"
                });
            }

            validateResult.AddToModelState(ModelState, null);
            return Json(new
            {
                error = true,
                message = "Check requirements!",
                state = ModelState.GetErrors()
            });
        }

        public IActionResult Faq()
        {
            var data = db.Faqs.Where(f => f.DeletedDate == null).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(Subscribe model)
        {
            if (model.Email != null)
            {
                if (!ModelState.IsValid)
                {
                    string msg = ModelState.Values.First().Errors[0].ErrorMessage;

                    return Json(new
                    {
                        error = true,
                        message = msg
                    });
                }

                var entity = db.Subscribes.FirstOrDefault(s => s.Email.Equals(model.Email));

                if (entity != null && entity.IsApproved == true)
                {
                    return Json(new
                    {
                        error = true,
                        message = "You have already subscribed!"
                    });
                }

                if (entity == null)
                {
                    db.Subscribes.Add(model);
                    db.SaveChanges();
                }
                else if (entity != null)
                {
                    model.Id = entity.Id;
                }

                string token = $"{model.Id}-{model.Email}-{Guid.NewGuid()}";

                token = crypto.Encrypt(token, true);




                string message = $"Confirm your subscription via the link: <a href='{Request.Scheme}://{Request.Host}/approve-subscribe?token={token}'>link</a> ";

                await emailService.SendMailAsync(model.Email, "Subscription Confirmation Email  ", message);

                return Json(new
                {
                    error = false,
                    message = "Confirmation email was sent to your email address!"
                });
            }
            else
            {
                return Json(new
                {
                    error = true,
                    message = "An email field have to be filled!"
                });
            }
        }

        [Route("/approve-subscribe")]
        public async Task<IActionResult> SubscribeApprove(string token)
        {
            //token = token.Decrypt(Program.key);
            token = crypto.Decrypt(token);

            Match match = Regex.Match(token, @"^(?<id>\d+)-(?<email>[^-]+)-(?<randomKey>.*)$");

            if (!match.Success)
            {
                //return "Invalid token!";
            }

            int id = Convert.ToInt32(match.Groups["id"].Value);
            string email = match.Groups["email"].Value;
            string randomKey = match.Groups["randomKey"].Value;

            var entity = db.Subscribes.FirstOrDefault(s => s.Id == id);

            if (entity == null)
            {
                //return "Not found!";
            }

            if (entity.IsApproved)
            {
                //return "Already approved!";
            }

            entity.IsApproved = true;
            entity.ApprovedDate = DateTime.UtcNow.AddHours(4);
            db.SaveChanges();

            return View();

            //return $"Id: {id} | Email: {email} | RandomKey: {randomKey}";
        }
    }
}
