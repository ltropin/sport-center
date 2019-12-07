using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using SportCenter.ViewModels;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepo;
        public AccountController(IAccountRepository accountRepo)
        {
            this.accountRepo = accountRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var hashPass = Client.HashPass(model.Password);
                var user = accountRepo.GetClient(model.Email, hashPass);
                if (user != null)
                {
                    Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные email и(или) пароль");
            }
            return RedirectToAction(nameof(Login), new { error = "Некорректные email и(или) пароль" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = accountRepo.FindOne(x => x.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    var allRoles = accountRepo.GetRoles().ToList();

                    accountRepo.Create(new Client
                    {
                        Email = model.Email,
                        Password = Client.HashPass(model.Password),
                        IdRole = FromRoleEnum(Roles.Client, allRoles).Id,
                        Fio = model.FIO
                    });

                    accountRepo.Save();

                    Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Некорректные email и(или) пароль");
            }
            return View(model);
        }

        private void Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetTrains()
        {
            var client = accountRepo.GetCurrentClient(User.Identity.Name);

            ViewData["GroupTrains"] = accountRepo.GetGroupTrainings(client.Id).ToList();
            ViewData["PersonalTrains"] = accountRepo.GetPersonalTrainings(client.Id).ToList();

            return View(Roles.Client);

        }
        [Authorize]
        [HttpGet]
        public IActionResult DeleteGroup(GroupTrainingModel groupTraining)
        {
            var client = accountRepo.GetCurrentClient(User.Identity.Name);

            accountRepo.DeleteOrderGroupById(groupTraining.ID, client.Id);
            accountRepo.Save();

            return RedirectToAction(nameof(GetTrains));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeletePersonal(PersonalTrainingModel personalTraining)
        {
            accountRepo.DeletePersonalTrainById(personalTraining.ID);
            accountRepo.Save();
            return RedirectToAction(nameof(GetTrains));
        }
    }
}