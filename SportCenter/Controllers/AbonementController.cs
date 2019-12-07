using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class AbonementController : Controller
    {
        private readonly IAbonementRepository abonementRepo;
        public AbonementController(IAbonementRepository abonementRepo)
        {
            this.abonementRepo = abonementRepo;
        }
        [Authorize]
        [HttpGet]
        public IActionResult RequestAbonement()
        {
            var client = abonementRepo.GetCurrentClient(User.Identity.Name);

            ViewData["MyRequests"] = abonementRepo.GetRequestAbonements(client.Id).ToList();
            ViewData["TermList"] = abonementRepo.SelectTerm(TermList);
            ViewData["TimeList"] = abonementRepo.SelectTime(TimeList);

            return View(Roles.Client);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RequestAbonement(RequestAbonement requestAbonement)
        {
            var client = abonementRepo.GetCurrentClient(User.Identity.Name);
            abonementRepo.AddRequestAbonement(requestAbonement, client.Id);

            abonementRepo.Save();

            return RedirectToAction(nameof(RequestAbonement));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeclineRequestAbonement(int Id)
        {
            abonementRepo.DeleteRequestAbonemetById(Id);

            abonementRepo.Save();

            return RedirectToAction(nameof(RequestAbonement));
        }

        [Authorize]
        [HttpGet]
        public IActionResult AbonementInfo()
        {
            var client = abonementRepo.GetCurrentClient(User.Identity.Name);
            ViewData["Abonement"] = abonementRepo.GetByClientId(client.Id);

            return View(Roles.Client);
        }

        [Authorize]
        [HttpGet]
        public IActionResult FreezeAbonement(long Id)
        {
            ViewData["Abonement"] = abonementRepo.GetById(Id);

            return View(Roles.Client);
        }
        [Authorize]
        [HttpPost]
        public IActionResult FreezeAbonement(Abonement abonement)
        {
            var client = abonementRepo.GetCurrentClient(User.Identity.Name);
            var newAbonement = abonementRepo.GetById(abonement.Number);

            abonementRepo.FreezeAbonement(newAbonement, abonement);

            abonementRepo.Save();

            return RedirectToAction(nameof(AbonementInfo));
        }
    }
}