using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Extensions;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using SportCenter.ViewModels;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class GroupTrainingController : Controller
    {
        private readonly IGroupTrainingRepository groupTrainRepo;
        public GroupTrainingController(IGroupTrainingRepository groupTrainRepo)
        {
            this.groupTrainRepo = groupTrainRepo;
        }

        [Authorize]
        public IActionResult FilterPage()
        {
            ViewData["Trainers"] = groupTrainRepo.GetTrainersSelect();
            ViewData["DayOfWeeks"] = DayOfWeeks;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult SelectGroupTraining(IFormCollection fc)
        {
            var idTrainer = int.Parse(fc["STrainer"].ToString());
            var dayOfWeek = int.Parse(fc["SDayOfWeek"].ToString());
            var client = groupTrainRepo.GetCurrentClient(User.Identity.Name);

            ViewData["Trainers"] = groupTrainRepo.GetTrainersSelect();
            ViewData["DayOfWeeks"] = groupTrainRepo.SelectedDayOfWeek(DayOfWeeks, dayOfWeek);
            ViewData["GroupTrainings"] = groupTrainRepo.GetGroupTrainingModels(dayOfWeek, idTrainer, client.Id);

            return View(Roles.Client);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddGroupTraining(string[] gtSelected)
        {
            var selectedIDGt = gtSelected.Select(int.Parse);
            var client = groupTrainRepo.GetCurrentClient(User.Identity.Name);
            
            groupTrainRepo.ReduceCapacity(selectedIDGt);
            groupTrainRepo.AddSelectedGT(selectedIDGt, client.Id);

            groupTrainRepo.Save();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Cancel(int IDGt)
        {
            var client = groupTrainRepo.GetCurrentClient(User.Identity.Name);

            var cancelOrder = groupTrainRepo.GetCancelOrderGroup(client.Id, IDGt);
            groupTrainRepo.GetById(IDGt).Capacity++;
            groupTrainRepo.DelteOrderGroup(cancelOrder);

            groupTrainRepo.Save();

            return RedirectToAction(actionName: "FilterPage", controllerName: "GroupTraining");
        }


    }
}