using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Controllers
{
    public class PersonalTrainingController : Controller
    {
        private readonly IPersonalTrainingRepository personalTrainRepo;
        public PersonalTrainingController(IPersonalTrainingRepository personalTrainRepo)
        {
            this.personalTrainRepo = personalTrainRepo;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddPersonalTraining()
        {
            ViewData["Trainers"] = personalTrainRepo.GetTrainersSelect();
            ViewData["DayOfWeeks"] = DayOfWeeks;
            ViewData["TimeRanges"] = TimeList.Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddPersonalTraining(PersonalTrain personalTrain)
        {
            
            personalTrain.IdClient = personalTrainRepo.GetCurrentClient(User.Identity.Name).Id;

            if (personalTrainRepo.ExistRecord(personalTrain))
                return RedirectToAction(nameof(ExistRecord));

            personalTrainRepo.Create(personalTrain);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult ExistRecord()
        {
            return View();
        }
    }
}
