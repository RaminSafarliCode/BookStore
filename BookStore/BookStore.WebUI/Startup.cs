using BookStore.Application.AppCode.Extenstions;
using BookStore.Application.AppCode.Services;
using BookStore.Domain.AppCode.Providers;
using BookStore.Domain.Hubs;
using BookStore.Domain.Models.DataContexts;
using BookStore.Domain.Models.Entities.Membership;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebUI
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDbContext<BookStoreDbContext>(cfg =>
                {
                cfg.UseSqlServer(configuration.GetConnectionString("cString"));
            })
            .AddIdentity<BookStoreUser, BookStoreRole>()
            .AddEntityFrameworkStores<BookStoreDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;

                cfg.Password.RequireDigit = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredUniqueChars = 1;
                cfg.Password.RequiredLength = 3;

                cfg.User.RequireUniqueEmail = true;
                cfg.SignIn.RequireConfirmedEmail = true;

                cfg.Lockout.MaxFailedAccessAttempts = 3;
                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 1, 0);
            });

            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";

                cfg.Cookie.Name = "bookstore";
                cfg.Cookie.HttpOnly = true;
                cfg.ExpireTimeSpan = new TimeSpan(0, 15, 0);
            });

            services.AddAuthentication();
            services.AddAuthorization(cfg =>
            {
                foreach (var policyName in Extension.policies)
                {
                    cfg.AddPolicy(policyName, p =>
                    {
                        p.RequireAssertion(handler =>
                        {
                            return handler.User.IsInRole("SuperAdmin")
                            || handler.User.HasClaim(policyName, "1");
                        });
                    });
                }
            });

            services.AddScoped<UserManager<BookStoreUser>>();
            services.AddScoped<SignInManager<BookStoreUser>>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IClaimsTransformation, AppClaimProvider>();


            services.AddRouting(cfg =>
            {
                cfg.LowercaseUrls = true;
            });

            services.Configure<EmailServiceOptions>(cfg =>
            {
                configuration.GetSection("emailAccount").Bind(cfg);
            });
            services.AddSingleton<EmailService>();

            services.Configure<CryptoServiceOptions>(cfg =>
            {
                configuration.GetSection("cryptograpy").Bind(cfg);
            });
            services.AddSingleton<CryptoService>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("BookStore."))
                .ToArray();
            services.AddMediatR(assemblies);
            services.AddValidatorsFromAssemblies(assemblies);

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    // W A R N I N G
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();
            //}

            app.SeedMembership();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default-forgot-password",
                    pattern: "forgot-password.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "ForgotPassword"
                    });

                endpoints.MapControllerRoute(
                    name: "default-reset-password.html",
                    pattern: "reset-password.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "ResetPassword"
                    });






                endpoints.MapControllerRoute(
                    name: "default-signin",
                    pattern: "signin.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "signin"

                    });

                endpoints.MapControllerRoute(
                    name: "default-register",
                    pattern: "register.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "register"
                    });

                endpoints.MapControllerRoute(
                    name: "default-register-confirm",
                    pattern: "registration-confirm.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "RegisterConfirm"
                    });

                endpoints.MapControllerRoute(
                    name: "default-profile",
                    pattern: "profile.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "profile"
                    });

                endpoints.MapControllerRoute(
                    name: "default-logout",
                    pattern: "logout.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "logout"
                    });

                endpoints.MapControllerRoute(
                    name: "default-accessdenied",
                    pattern: "accessdenied.html",
                    defaults: new
                    {
                        area = "",
                        controller = "account",
                        action = "accessdenied"
                    });

                endpoints.MapAreaControllerRoute("defaultAdmin", "admin", "admin/{controller=aboutcompany}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}
