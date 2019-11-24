using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportCenter.Data;
using SportCenter.ViewModels.GroupTraining;

namespace SportCenter.Controllers
{
    public class GroupTrainingController : Controller
    {
        private readonly SportCenterContext context;
        public GroupTrainingController(SportCenterContext context)
        {
            this.context = context;
        }

        public IActionResult SelectDay()
        {

            ViewData["NDays"] = Enum.GetValues(typeof(DayOfWeek))
                                    .Cast<DayOfWeek>()
                                    .Select(x => (Number: (int)x + 1, Day: Enum.GetName(typeof(DayOfWeek), x)))
                                    .ToList();
            return View();
        }
        [HttpPost]
        public IActionResult SelectTrainer(IFormCollection fc)
        {
            var selectedDay = int.Parse(fc["selectDay"].ToString());
            //context.Trainer.First().

            return View();
        }
    }
}