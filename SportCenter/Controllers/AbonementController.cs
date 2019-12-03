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
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class AbonementController : Controller
    {
        private readonly SportCenterContext context;
        public AbonementController(SportCenterContext context)
        {
            this.context = context;
        }
        [Authorize]
        [HttpGet]
        public IActionResult RequestAbonement()
        {
            var currentClient = context.Client.Single(x => x.Email == User.Identity.Name);
            ViewData["MyRequests"] = context.RequestAbonement.Where(x => x.IdClient == currentClient.Id).ToList();
            ViewData["TermList"] = TermList.Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            ViewData["TimeList"] = TimeList.Select(x => new SelectListItem { Text = x.ToString(@"hh\:mm"), Value = x.ToString() });

            return View(Roles.Client);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RequestAbonement(RequestAbonement requestAbonement)
        {
            var currentClient = context.Client.Single(x => x.Email == User.Identity.Name);
            requestAbonement.IdClient = currentClient.Id;
            context.RequestAbonement.Add(requestAbonement);
            context.SaveChanges();

            return RedirectToAction(nameof(RequestAbonement));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeclineRequestAbonement(int Id)
        {
            var requestAbonement = context.RequestAbonement.Single(x => x.Id == Id);

            context.RequestAbonement.Remove(requestAbonement);

            context.SaveChanges();

            return RedirectToAction(nameof(RequestAbonement));
        }

        [Authorize]
        [HttpGet]
        public IActionResult AbonementInfo()
        {
            var currentClient = context.Client.Single(x => x.Email == User.Identity.Name);
            ViewData["Abonement"] = context.Abonement.SingleOrDefault(x => x.IdClient == currentClient.Id);

            return View(Roles.Client);
        }

        [Authorize]
        [HttpGet]
        public IActionResult FreezeAbonement(long Id)
        {
            var currentClient = context.Client.Single(x => x.Email == User.Identity.Name);
            ViewData["Abonement"] = context.Abonement.Single(x => x.Number == Id);

            return View(Roles.Client);
        }
        [Authorize]
        [HttpPost]
        public IActionResult FreezeAbonement(Abonement abonement)
        {
            var currentClient = context.Client.Single(x => x.Email == User.Identity.Name);
            var newAbonement = context.Abonement.Single(x => x.Number == abonement.Number);

            newAbonement.Capacity -= abonement.IntervalBlock.Value;
            newAbonement.DateBlock = abonement.DateBlock.Value;
            newAbonement.IntervalBlock = abonement.IntervalBlock.Value;
            newAbonement.IsActive = false;

            context.SaveChanges();

            return RedirectToAction(nameof(AbonementInfo));
        }
    }
}