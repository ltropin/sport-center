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
using SportCenter.ViewModels;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class AccountController : Controller
    {
        private readonly SportCenterContext context;

        public AccountController(SportCenterContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var hashPass = Client.HashPass(model.Password);
                var user = await context.Client.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == hashPass);
                if (user != null)
                {
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные email и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.Client.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    var allRoles = context.Role.ToList();
                    context.Client.Add(new Client { Email = model.Email,
                                                    Password = Client.HashPass(model.Password),
                                                    IdRole = FromRoleEnum(Roles.Client, allRoles).Id,
                                                    Fio = model.FIO });
                    await context.SaveChangesAsync();

                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Некорректные email и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
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
            var clientID = context.Client.Single(x => x.Email == User.Identity.Name).Id;
            var groupTrains = context.OrderGroup
                                     .Include(x => x.IdGroupTrainNavigation)
                                     .Where(group => group.IdClient == clientID)
                                     .Select(group => group.IdGroupTrainNavigation)
                                     .AsNoTracking()
                                     .Select(x => new GroupTrainingModel
                                     {
                                         ID = x.Id,
                                         Name = x.Name,
                                         TrainerName = x.IdTrainerNavigation.Fio,
                                         Capacity = x.Capacity,
                                         DayOfWeek = DayOfWeekMap[x.DayOfWeek].Long,
                                         Recorded = true,
                                         Time = x.Time.ToString(@"hh\:mm")
                                     })
                                     .ToList();
            var personalTrains = context.PersonalTrain
                                        .Include(x => x.IdTrainerNavigation)
                                        .Where(x => x.IdClient == clientID)
                                        .AsNoTracking()
                                        .Select(x => new PersonalTrainingModel
                                        {
                                            ID = x.Id,
                                            TrainerName = x.IdTrainerNavigation.Fio,
                                            DayOfWeek = DayOfWeekMap[x.DayOfWeek].Long,
                                            Time = x.Time.ToString(@"hh\:mm")
                                        })
                                        .ToList();

            ViewData["GroupTrains"] = groupTrains;
            ViewData["PersonalTrains"] = personalTrains;

            return View(Roles.Client);
                                        
        }
        [Authorize]
        [HttpGet]
        public IActionResult DeleteGroup(GroupTrainingModel groupTraining)
        {
            var clientID = context.Client.Single(x => x.Email == User.Identity.Name).Id;
            var deleteItem = context.OrderGroup.Single(x => x.IdGroupTrain == groupTraining.ID && x.IdClient == clientID);

            context.OrderGroup.Attach(deleteItem);
            context.OrderGroup.Remove(deleteItem);

            context.SaveChanges();

            return RedirectToAction(nameof(GetTrains));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeletePersonal(PersonalTrainingModel personalTraining)
        {
            var deleteItem = context.PersonalTrain.Single(x => x.Id == personalTraining.ID);
            context.PersonalTrain.Attach(deleteItem);
            context.PersonalTrain.Remove(deleteItem);

            context.SaveChanges();
            return RedirectToAction(nameof(GetTrains));
        }
    }
}