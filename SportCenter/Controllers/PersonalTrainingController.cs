using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Data;
using SportCenter.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportCenter.Controllers
{
    public class PersonalTrainingController : Controller
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

            return context.Trainer.Select(x => new SelectListItem
            {
                Text = x.Fio,
                Value = x.Id.ToString()
            }).ToList();
        }

        public PersonalTrainingController(SportCenterContext context)
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
        [HttpGet]
        public IActionResult AddPersonalTraining()
        {
            ViewData["Trainers"] = GetTrainerList();
            ViewData["DayOfWeeks"] = DayOfWeeks;
            ViewData["TimeRanges"] = Enumerable.Range(0, 9).Select(x => new TimeSpan(8, 0, 0) + x * new TimeSpan(1, 30, 0))
                                               .Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddPersonalTraining(PersonalTrain personalTrain)
        {
            personalTrain.IdClient = context.Client.Single(x => x.Email == User.Identity.Name).Id;
            context.PersonalTrain.Add(personalTrain);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
