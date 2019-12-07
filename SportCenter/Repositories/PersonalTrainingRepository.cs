using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories
{
    public class PersonalTrainingRepository : Repository<PersonalTrain>, IPersonalTrainingRepository
    {
        public PersonalTrainingRepository(SportCenterContext context) : base(context) { }

        public bool ExistRecord(PersonalTrain personalTrain)
        {
            return context.PersonalTrain.Any(x => x.IdTrainer == personalTrain.IdTrainer &&
                                             x.Time == personalTrain.Time &&
                                             x.DayOfWeek == personalTrain.DayOfWeek);
        }

        public IEnumerable<SelectListItem> GetTrainersSelect()
        {
            return context.Trainer.Select(x => new SelectListItem
            {
                Text = x.Fio,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}
