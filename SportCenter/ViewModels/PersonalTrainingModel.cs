using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.ViewModels
{
    public class PersonalTrainingModel
    {
        public int ID { get; set; }
        public string TrainerName { get; set; }
        public string Time { get; set; }
        public string DayOfWeek { get; set; }
    }
}
