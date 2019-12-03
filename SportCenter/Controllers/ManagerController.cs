using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Models;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class ManagerController : Controller
    {
        private readonly SportCenterContext context;

        public ManagerController(SportCenterContext context)
        {
            this.context = context;
        }
        [Authorize]
        public IActionResult RequestsManage()
        {
            var currentClient = context.Client.Include(x => x.IdRoleNavigation)
                                              .Single(x => x.Email == User.Identity.Name);

            if (FromRoleElement(currentClient.IdRoleNavigation) != Roles.Manager)
                return RedirectToAction("NoAccess", "Home");

            var clientRoleId = FromRoleEnum(Roles.Client, context.Role.ToList()).Id;
            var requests = context.RequestAbonement
                                 .Include(x => x.IdClientNavigation)
                                 .Where(x => x.IdClientNavigation.IdRole == clientRoleId)
                                 .ToList();

            ViewData["RequestAbonements"] = requests;

            return View(Roles.Manager);
        }
        [Authorize]
        [HttpGet]
        public IActionResult AcceptAbonement(int Id)
        {
            var requestAbonement = context.RequestAbonement.Single(x => x.Id == Id);

            context.Abonement.Add(new Abonement
            {
                IdClient = requestAbonement.IdClient,
                Capacity = 30,
                Term = requestAbonement.Term,
                Time = requestAbonement.Time,
                IsActive = true
            });
            context.RequestAbonement.Remove(requestAbonement);

            context.SaveChanges();

            return RedirectToAction(nameof(RequestsManage));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeclineAbonement(int Id)
        {
            var requestAbonement = context.RequestAbonement.Single(x => x.Id == Id);

            context.RequestAbonement.Remove(requestAbonement);

            context.SaveChanges();

            return RedirectToAction(nameof(RequestsManage));
        }
    }
}