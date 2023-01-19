using BookStore.Application.AppCode.Extenstions;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities;
using BookStore.Domain.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookStoreDbContext db;
        private readonly IValidator<ContactPost> contactPostValidator;

        public HomeController(BookStoreDbContext db, IValidator<ContactPost> contactPostValidator)
        {
            this.db = db;
            this.contactPostValidator = contactPostValidator;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
    }
}
