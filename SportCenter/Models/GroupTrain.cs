using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportCenter.Models
{
    public partial class GroupTrain
    {
        public GroupTrain()
        {
            OrderGroup = new HashSet<OrderGroup>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ID_Trainer")]
        public int IdTrainer { get; set; }
        [Required]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int DayOfWeek { get; set; }

        [ForeignKey(nameof(IdTrainer))]
        [InverseProperty(nameof(Trainer.GroupTrain))]
        public virtual Trainer IdTrainerNavigation { get; set; }
        [InverseProperty("IdGroupTrainNavigation")]
        public virtual ICollection<OrderGroup> OrderGroup { get; set; }
    }
}
