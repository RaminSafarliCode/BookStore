using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Services;
using BookStore.Domain.Models.Entities.Membership;
using BookStore.Domain.Models.FormData;
using BookStore.Domain.Models.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class AccountContoller : Controller
    {
        readonly SignInManager<BookStoreUser> signInManager;
        readonly UserManager<BookStoreUser> userManager;
        private readonly EmailService emailService;

        public AccountContoller(SignInManager<BookStoreUser> signInManager, UserManager<BookStoreUser> userManager, EmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
        }
        [AllowAnonymous]
        [Route("/signin.html")]
        public IActionResult Signin()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/signin.html")]
        public async Task<IActionResult> Signin(LoginFormModel user)
        {
            if (ModelState.IsValid)
            {
                BookStoreUser foundedUser = null;

                if (user.UserName.IsEmail())
                {
                    foundedUser = await userManager.FindByEmailAsync(user.UserName);
                }
                else
                {
                    foundedUser = await userManager.FindByNameAsync(user.UserName);
                }

                if (foundedUser == null)
                {
                    ViewBag.Message = "Your username or password is incorrect!";
                    goto end;
                }

                var signInResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true);

                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "Your username or password is incorrect!";
                    goto end;
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(new
                    {
                        error = false,
                        message = "login is completed"
                    });
                }


                var callbackUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(callbackUrl))
                {
                    return Redirect(callbackUrl);
                }

                return RedirectToAction("Index", "Home");
            }

        end:
            return View();
        }

        [AllowAnonymous]
        [Route("/register.html")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/register.html")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new BookStoreUser();

                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;
                //user.EmailConfirmed = true;

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string path = $"{Request.Scheme}://{Request.Host}/registration-confirm.html?email={user.Email}&token={token}";

                    var emailResponse = await emailService.SendMailAsync(user.Email, "Registration for BookStore", $"Confirm your subscription with this <a href='{path}'>link</a>.");

                    if (emailResponse)
                    {
                        ViewBag.Message = "Registration is successfully completed";
                    }
                    else
                    {
                        ViewBag.Message = "An error has occurred while sending email!";
                    }

                    //await userManager.AddToRoleAsync(user, "user");
                    //await signInManager.SignInAsync(user, false);

                    return RedirectToAction(nameof(Signin));
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [Route("registration-confirm.html")]
        public async Task<IActionResult> RegisterConfirm(string email, string token)
        {
            var foundedUser = await userManager.FindByEmailAsync(email);
            if (foundedUser == null)
            {
                ViewBag.Message = "Invalid token";
                goto end;
            }
            token = token.Replace(" ", "+");
            var result = await userManager.ConfirmEmailAsync(foundedUser, token);

            if (!result.Succeeded)
            {
                ViewBag.Message = "Invalid token";
                goto end;
            }

            ViewBag.Message = "Your account is approved!";
        end:
            return RedirectToAction(nameof(Signin));
        }

        [AllowAnonymous]
        [Route("/forgot-password.html")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/forgot-password.html")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(BookStoreForgotPassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var foundedUser = await userManager.FindByEmailAsync(model.Email);

            if (foundedUser == null)
            {
                ViewBag.Message = "The user was not found!";
                goto end;
            }

            if (foundedUser != null && await userManager.IsEmailConfirmedAsync(foundedUser))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(foundedUser);
                token = token.Replace(" ", "+");
                string path = $"{Request.Scheme}://{Request.Host}/reset-password.html?email={foundedUser.Email}&token={token}";

                await emailService.SendMailAsync(foundedUser.Email, "Reset Password", $"To reset your password click this <a href='{path}'>link</a>!");
                return View("ForgotPasswordConfirmation");
            }

            end:
            return View(model);
        }

        [AllowAnonymous]
        [Route("reset-password.html")]
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel();
            model.Token = token.Replace(" ", "+");
            model.Email = email;
            return View(model);
        }

        [AllowAnonymous]
        [Route("reset-password.html")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var foundedUser = await userManager.FindByEmailAsync(model.Email);

            if (foundedUser == null)
            {
                return View(model);
            }
            var result = await userManager.ResetPasswordAsync(foundedUser, model.Token.Replace(" ", "+"), model.Password);

            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [Route("/profile.html")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("/logout.html")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Signin", "Account");
        }

        [AllowAnonymous]
        [Route("/accessdenied.html")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
