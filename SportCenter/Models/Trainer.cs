using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class Trainer
    {
        public Trainer()
        {
            GroupTrain = new HashSet<GroupTrain>();
            PersonalTrain = new HashSet<PersonalTrain>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Required]
        [Column("FIO")]
        public string Fio { get; set; }

        [InverseProperty("IdTrainerNavigation")]
        public virtual ICollection<GroupTrain> GroupTrain { get; set; }
        [InverseProperty("IdTrainerNavigation")]
        public virtual ICollection<PersonalTrain> PersonalTrain { get; set; }
    }
}
