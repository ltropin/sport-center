using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Data;
using SportCenter.Models;
using static SportCenter.Extensions.Extensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportCenter.Controllers
{
    public class PersonalTrainingController : Controller
    {
        private readonly SportCenterContext context;

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
        }
        [Authorize]
        [HttpGet]
        public IActionResult AddPersonalTraining()
        {
            ViewData["Trainers"] = GetTrainerList();
            ViewData["DayOfWeeks"] = DayOfWeeks;
            ViewData["TimeRanges"] = TimeList.Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
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
