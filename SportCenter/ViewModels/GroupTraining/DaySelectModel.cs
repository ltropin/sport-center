using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportCenter.ViewModels.GroupTraining
{
    public class DaySelectModel
    {
        [Required(ErrorMessage = "Выберите день недели")]
        [Range(1, 7)]
        public int DayOfWeek { get; set; }
    }
}
