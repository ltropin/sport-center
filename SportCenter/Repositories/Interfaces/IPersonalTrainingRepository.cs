using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories.Interfaces
{
    public interface IPersonalTrainingRepository : IRepository<PersonalTrain>
    {
        IEnumerable<SelectListItem> GetTrainersSelect();
        bool ExistRecord(PersonalTrain personalTrain);
    }
}
