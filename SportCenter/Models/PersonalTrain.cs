using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class PersonalTrain
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan Time { get; set; }
        [Column("ID_Trainer")]
        public int IdTrainer { get; set; }
        [Column("ID_Client")]
        public int IdClient { get; set; }

        [ForeignKey(nameof(IdClient))]
        [InverseProperty(nameof(Client.PersonalTrain))]
        public virtual Client IdClientNavigation { get; set; }
        [ForeignKey(nameof(IdTrainer))]
        [InverseProperty(nameof(Trainer.PersonalTrain))]
        public virtual Trainer IdTrainerNavigation { get; set; }
    }
}
