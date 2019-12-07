using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerRepository managerRepo;
        public ManagerController(IManagerRepository managerRepo)
        {
            this.managerRepo = managerRepo;
        }
        [Authorize]
        public IActionResult RequestsManage()
        {
            var client = managerRepo.GetCurrentClient(User.Identity.Name, withRole: true);

            if (FromRoleElement(client.IdRoleNavigation) != Roles.Manager)
                return RedirectToAction("NoAccess", "Home");

            var clientRoleId = FromRoleEnum(Roles.Client, managerRepo.GetRoles().ToList()).Id;

            ViewData["RequestAbonements"] = managerRepo.GetRequestAbonements(clientRoleId);

            return View(Roles.Manager);
        }
        [Authorize]
        [HttpGet]
        public IActionResult AcceptAbonement(int Id)
        {
            var requestAbonement = managerRepo.GetById(Id);

            managerRepo.AddAbonement(requestAbonement);

            managerRepo.Save();

            return RedirectToAction(nameof(RequestsManage));
        }

        [Authorize]
        [HttpGet]
        public IActionResult DeclineAbonement(int Id)
        {
            var requestAbonement = managerRepo.GetById(Id);

            managerRepo.Delete(requestAbonement);

            managerRepo.Save();

            return RedirectToAction(nameof(RequestsManage));
        }
    }
}