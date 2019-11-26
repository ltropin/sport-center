using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Extensions;
using SportCenter.Models;
using SportCenter.ViewModels;

namespace SportCenter.Controllers
{
    public class GroupTrainingController : Controller
    {
        private readonly SportCenterContext context;
        private readonly IEnumerable<SelectListItem> DayOfWeeks;
        private readonly Dictionary<int, (string Short, string Long)> DayOfWeekMap = new Dictionary<int, (string Short, string Long)>
        {
            [0] = (Short: "Пн", Long: "Понедельник"),
            [1] = (Short: "Вт", Long: "Вторник"),
            [2] = (Short: "Ср", Long: "Среда"),
            [3] = (Short: "Чт", Long: "Четверг"),
            [4] = (Short: "Пт", Long: "Пятница"),
            [5] = (Short: "Сб", Long: "Суббота"),
            [6] = (Short: "Вс", Long: "Восскресенье"),
        };

        public List<SelectListItem> GetTrainerList()
        {
            var trainers = context.Trainer.Select(x => new SelectListItem
            {
                Text = x.Fio,
                Value = x.Id.ToString()
            }).ToList();
            trainers.Insert(0, new SelectListItem { Text = "Все", Selected = true, Value = "0" });
            return trainers;
        }

        public GroupTrainingController(SportCenterContext context)
        {
            this.context = context;
            DayOfWeeks = Enum.GetValues(typeof(DayOfWeek))
                                         .Cast<DayOfWeek>()
                                         .Select(x => new SelectListItem
                                         {
                                             Text = DayOfWeekMap[(int)x].Long,
                                             Value = ((int)x).ToString()
                                         });
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
            var groupTrainings = context.GroupTrain.Include(x => x.IdTrainerNavigation)
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

            return View(groupTrainings);
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