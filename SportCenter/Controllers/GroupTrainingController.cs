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
using SportCenter.ViewModels;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class GroupTrainingController : Controller
    {
        private readonly SportCenterContext context;

        public List<SelectListItem> GetTrainerList()
        {
            var trainers = context.Trainer
                .Select(x => new SelectListItem
                {
                    Text = x.Fio,
                    Value = x.Id.ToString()
                })
                .AsNoTracking()
                .ToList();
            trainers.Insert(0, new SelectListItem { Text = "Все", Selected = true, Value = "0" });
            return trainers;
        }

        public GroupTrainingController(SportCenterContext context)
        {
            this.context = context;
        }
        [Authorize]
        public IActionResult FilterPage()
        {
            ViewData["Trainers"] = GetTrainerList();
            ViewData["DayOfWeeks"] = DayOfWeeks;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult SelectGroupTraining(IFormCollection fc)
        {
            var idTrainer = int.Parse(fc["STrainer"].ToString());
            var dayOfWeek = int.Parse(fc["SDayOfWeek"].ToString());
            
            ViewData["Trainers"] = GetTrainerList();
            ViewData["DayOfWeeks"] = DayOfWeeks.Select(x => new SelectListItem { Text = x.Text,
                                                                                 Value = x.Value,
                                                                                 Selected = int.Parse(x.Value) == dayOfWeek });
            var clientId = context.Client.Single(x => x.Email == User.Identity.Name).Id;
            ViewData["GroupTrainings"] = context.GroupTrain.Include(x => x.IdTrainerNavigation)
                                                   .Include(x => x.OrderGroup)
                                                   .Where(gt => gt.DayOfWeek == dayOfWeek &&
                                                                (gt.IdTrainer == idTrainer || idTrainer == 0) &&
                                                                gt.Capacity > 0)
                                                   .ToList()
                                                   .Select(x => new GroupTrainingModel
                                                   {
                                                       ID = x.Id,
                                                       Name = x.Name,
                                                       Capacity = x.Capacity,
                                                       DayOfWeek = DayOfWeekMap[x.DayOfWeek].Short,
                                                       Time = x.Time.ToString(),
                                                       TrainerName = x.IdTrainerNavigation.Fio,
                                                       Recorded = x.OrderGroup.FirstOrDefault(x => x.IdClient == clientId) != null
                                                   })
                                                   .TakeMany(3);

            return View(Roles.Client);
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddGroupTraining(string[] gtSelected)
        {
            var selectedIDGt = gtSelected.Select(int.Parse);
            selectedIDGt.Select(idGt => context.GroupTrain.Single(x => x.Id == idGt))
                        .ToList()
                        .ForEach(gt => gt.Capacity--);
            var clientId = context.Client.Single(x => x.Email == User.Identity.Name).Id;
            selectedIDGt.ToList().ForEach(idGt =>
            {
                context.OrderGroup.Add(new OrderGroup
                {
                    IdClient = clientId,
                    IdGroupTrain = idGt,
                    Date = DateTime.Now
                });
            });

            context.SaveChanges();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Cancel(int IDGt)
        {
            var clientId = context.Client.Single(x => x.Email == User.Identity.Name).Id;

            var cancelOrder = context.OrderGroup.Single(x => x.IdClient == clientId && x.IdGroupTrain == IDGt);
            context.GroupTrain.Single(x => x.Id == IDGt).Capacity++;
            context.OrderGroup.Attach(cancelOrder);
            context.OrderGroup.Remove(cancelOrder);

            context.SaveChanges();
            return RedirectToAction(actionName: "FilterPage", controllerName: "GroupTraining");
        }


    }
}